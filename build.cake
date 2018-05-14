#addin nuget:?package=Cake.Docker&version=0.9.3

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument<string>("target", "Default");
var configuration = Argument<string>("configuration", "Release");
var verbosity = Argument<string>("verbosity", "Minimal");

///////////////////////////////////////////////////////////////////////////////
// GLOBAL VARIABLES
///////////////////////////////////////////////////////////////////////////////

var sourceDir = Directory("./src");
var testsDir = Directory("./tests");

var solutions = GetFiles("./**/*.sln");
var publishProjects = new []
{
    sourceDir.Path + "/Lambda/Lambda.csproj",
    testsDir.Path + "/Lambda.Api.Tests/Lambda.Api.Tests.csproj"
};

var lambdaProjects = new []
{
    sourceDir.Path + "/Lambda/Lambda.csproj"
};

var unitTestsProjects = GetFiles(testsDir.Path + "/**/*.Tests.Unit.csproj");
var acceptanceTestsProjects = GetFiles(testsDir.Path + "/**/*.Tests.Acceptance.csproj");

// DOCKER
var dockerImageName = "joaoasrosa/testing-api";
var dockerContainerName = "testing-api";
var dockerNetworkName = "testing";

// BUILD OUTPUT DIRECTORIES
var artifactsDir = Directory("./artifacts");
var publishDir = Directory("./publish/");

// LOCAL DEPLOY OUTPUT DIRECTORIES
var localDeployDir = Directory("./local-deploy");

// VERBOSITY
var dotNetCoreVerbosity = Cake.Common.Tools.DotNetCore.DotNetCoreVerbosity.Normal;
if (!Enum.TryParse(verbosity, true, out dotNetCoreVerbosity))
{	
    dotNetCoreVerbosity = Cake.Common.Tools.DotNetCore.DotNetCoreVerbosity.Normal;
    Warning(
        "Verbosity could not be parsed into type 'Cake.Common.Tools.DotNetCore.DotNetCoreVerbosity'. Defaulting to {0}", 
        dotNetCoreVerbosity); 
}

///////////////////////////////////////////////////////////////////////////////
// COMMON FUNCTION DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

void Test(FilePathCollection testProjects)
{
    var settings = new DotNetCoreTestSettings
	{
		Configuration = configuration,
		NoBuild = true,
		NoRestore = true,
        Verbosity = dotNetCoreVerbosity
	};

	foreach(var testProject in testProjects)
    {
		Information("Testing '{0}'...",  testProject.FullPath);
		DotNetCoreTest(testProject.FullPath, settings);
		Information("'{0}' has been tested.", testProject.FullPath);
	}
}

string GetProjectName(string project)
{
    return project
        .Split(new [] {'/'}, StringSplitOptions.RemoveEmptyEntries)
        .Last()
        .Replace(".csproj", string.Empty);
}

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
    // Executed BEFORE the first task.
	EnsureDirectoryExists(artifactsDir);
	EnsureDirectoryExists(publishDir);
	EnsureDirectoryExists(localDeployDir);
    Information("Running tasks...");
});

Teardown(ctx =>
{
    // Executed AFTER the last task.
    Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Clean")
    .Description("Cleans all directories that are used during the build process.")
    .Does(() =>
    {
        foreach(var solution in solutions)
        {
            Information("Cleaning {0}", solution.FullPath);
            CleanDirectories(solution.FullPath + "/**/bin/" + configuration);
            CleanDirectories(solution.FullPath + "/**/obj/" + configuration);
            Information("{0} was clean.", solution.FullPath);
        }

        CleanDirectory(artifactsDir);
        CleanDirectory(publishDir);
        CleanDirectory(localDeployDir);
    });

Task("Restore")
	.Description("Restores all the NuGet packages that are used by the specified solution.")
	.Does(() => 
    {
        var settings = new DotNetCoreRestoreSettings
        {
            DisableParallel = false,
            NoCache = true,
            Verbosity = dotNetCoreVerbosity
        };
        
        foreach(var solution in solutions)
        {
            Information("Restoring NuGet packages for '{0}'...", solution);
            DotNetCoreRestore(solution.FullPath, settings);
            Information("NuGet packages restored for '{0}'.", solution);
        }
    });

Task("Build")
	.Description("Builds all the different parts of the project.")
	.Does(() => 
    {
        var msBuildSettings = new DotNetCoreMSBuildSettings 
        {
            TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error,
            Verbosity = dotNetCoreVerbosity
        };

        var settings = new DotNetCoreBuildSettings 
        {
            Configuration = configuration,
            MSBuildSettings = msBuildSettings,
            NoRestore = true
        };

        foreach(var solution in solutions)
        {
            Information("Building '{0}'...", solution);
            DotNetCoreBuild(solution.FullPath, settings);
            Information("'{0}' has been built.", solution);
        }
    });

Task("Test-Unit")
    .Description("Runs all your unit tests, using dotnet CLI.")
    .Does(() => { Test(unitTestsProjects); });

Task("Test-Acceptance")
    .Description("Runs all your acceptance tests, using dotnet CLI.")
    .Does(() => { Test(acceptanceTestsProjects); });

Task("Publish")
    .Description("Publish the Lambda Functions.")
    .Does(() => 
    {
        foreach(var project in publishProjects)
        {
			var projectName =  project
				.Split(new [] {'/'}, StringSplitOptions.RemoveEmptyEntries)
				.Last()
				.Replace(".csproj", string.Empty);

            var outputDirectory = System.IO.Path.Combine(publishDir, projectName);

            var msBuildSettings = new DotNetCoreMSBuildSettings 
            {
                TreatAllWarningsAs = MSBuildTreatAllWarningsAs.Error,
                Verbosity = dotNetCoreVerbosity
            };

			var settings = new DotNetCorePublishSettings
			{
				Configuration = configuration,
				MSBuildSettings = msBuildSettings,
				NoRestore = true,
				OutputDirectory = outputDirectory,
				Verbosity = dotNetCoreVerbosity
			};

            Information("Publishing '{0}'...", projectName);
            DotNetCorePublish(project, settings); 
            Information("'{0}' has been published.", projectName);
        }
    });

Task("Pack")
	.Description("Packs all the different parts of the project.")
	.Does(() => 
    {
		foreach(var project in lambdaProjects)
        {
			var projectName = GetProjectName(project);

			Information("Packing '{0}'...", projectName);
			var path = System.IO.Path.Combine(publishDir, projectName);
			var files = GetFiles(path + "/*.*");
			Zip(
				path, 
				System.IO.Path.Combine(artifactsDir, $"{projectName}.zip"),
				files);
			Information("'{0}' has been packed.", projectName);
        }
    });
    
Task("Deploy-Local")
	.Description("Deploys all the project parts locally.")
	.Does(() => 
    {
        foreach(var project in lambdaProjects)
        {
            var projectName = GetProjectName(project);
        
            var file = System.IO.Path.Combine(artifactsDir, $"{projectName}.zip");
            
            Information("Copying '{0} to '{1}'...", file, localDeployDir);
            CopyFileToDirectory(file, localDeployDir);
            Information("'{0} has been copyed to '{1}'.", file, localDeployDir);
        }
        
        var samTemplate = "./build/template.yml";
        
        Information("Copying '{0} to '{1}'...", samTemplate, localDeployDir);
        CopyFileToDirectory(samTemplate, localDeployDir);
        Information("'{0} has been copyed to '{1}'.", samTemplate, localDeployDir);
        
        var eventJson = "./build/event.json";
        
        Information("Copying '{0} to '{1}'...", eventJson, localDeployDir);
        CopyFileToDirectory(eventJson, localDeployDir);
        Information("'{0} has been copyed to '{1}'.", eventJson, localDeployDir);
    });

Task("Create-Network")
	.Description("Creates a Docker network.")
	.Does(() => 
    {
        Information("Creating the Docker network '{0}'.", dockerNetworkName);
        DockerNetworkCreate(dockerNetworkName);
        Information("Docker network '{0}' has been created.", dockerNetworkName);
    });

Task("Create-Container")
	.Description("Creates a Docker container for the Testing API.")
	.Does(() => 
    {
        string dockerfile = "./build/dockerfile";
        string outputDirectory = System.IO.Path.Combine(publishDir, "Lambda.Api.Tests");
                    
        Information("Copying '{0} to '{1}'...", dockerfile, outputDirectory);
        CopyFileToDirectory(dockerfile, outputDirectory);
        Information("'{0} has been copyed to '{1}'.", outputDirectory, outputDirectory);
        
        var settings = new DockerImageBuildSettings
        {
            Tag = new[] { dockerImageName },
            ForceRm = true,
            Pull = true
        };
        
        Information("Building the container '{0}'.", dockerImageName);
        DockerBuild(settings, outputDirectory);
        Information("Container '{0}' has been built.", dockerImageName);
    });

Task("Remove-Network")
	.Description("Removes the Docker network.")
	.Does(() => 
    {
        Information("Removing the Docker network '{0}'.", dockerNetworkName);
        DockerNetworkRemove(dockerNetworkName);
        Information("Docker network '{0}' has been removed.", dockerNetworkName);
    });

Task("Remove-Container-Image")
	.Description("Removes the Docker container image.")
	.Does(() => 
    {
        Information("Removing the container image '{0}'.", dockerImageName);
        DockerRmi(dockerImageName);
        Information("Container image '{0}' has been removed.", dockerImageName);
    });

///////////////////////////////////////////////////////////////////////////////
// TARGETS
///////////////////////////////////////////////////////////////////////////////

Task("Package")
    .Description("This is the task which will run if target Package is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .Does(() => { Information("Package target ran."); });
    
Task("Test-Local")
    .Description("First runs Build, then Test targets and finally the Acceptance Tests.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .IsDependentOn("Deploy-Local")
    .IsDependentOn("Create-Container")
    .IsDependentOn("Create-Network")
    .IsDependentOn("Test-Acceptance")
    .IsDependentOn("Remove-Network")
    .IsDependentOn("Remove-Container-Image")
    .Does(() => { Information("Tested everything"); });

Task("Clean-Docker")
    .Description("Clean Docker artifacts")
    .IsDependentOn("Remove-Network")
    .IsDependentOn("Remove-Container-Image")
    .Does(() => { Information("Cleaned everything"); });

Task("AppVeyor")
    .Description("This is the task which will run if target AppVeyor is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .IsDependentOn("Deploy-Local")
    .IsDependentOn("Create-Container")
    .IsDependentOn("Create-Network")
    .IsDependentOn("Test-Acceptance")
    .IsDependentOn("Remove-Network")
    .IsDependentOn("Remove-Container-Image")
    .Does(() => { Information("AppVeyor target ran."); });

Task("TravisCI")
    .Description("This is the task which will run if target TravisCI is passed in.")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("Build")
    .IsDependentOn("Test-Unit")
    .IsDependentOn("Publish")
    .IsDependentOn("Pack")
    .IsDependentOn("Deploy-Local")
    .IsDependentOn("Create-Container")
    .IsDependentOn("Create-Network")
    .IsDependentOn("Test-Acceptance")
    .IsDependentOn("Remove-Network")
    .IsDependentOn("Remove-Container-Image")
    .Does(() => { Information("TravisCI target ran."); });
    
Task("Default")
    .Description("This is the default task which will run if no specific target is passed in.")
    .IsDependentOn("Package");

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);