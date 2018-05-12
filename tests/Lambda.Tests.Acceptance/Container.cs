using System.Diagnostics;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;

namespace Lambda.Tests.Acceptance
{
    internal class Container
    {
        private readonly string _apiBehaviour;
        private bool _isTearDown;

        internal Container(
            string apiBehaviour)
        {
            _apiBehaviour = apiBehaviour;
        }

        internal void Start()
        {
            var arguments =
                $"run -d -i --name testing-api --network testing -p 8000:80 -e API_BEHAVIOUR={_apiBehaviour} joaoasrosa/testing-api";

            RunDockerCommand(arguments);
        }

        internal void Stop()
        {
            _isTearDown = true;

            var arguments = "stop testing-api";

            RunDockerCommand(arguments);

            arguments = "rm testing-api";

            RunDockerCommand(arguments);
        }

        private void RunDockerCommand(string arguments)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "docker";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.OutputDataReceived += ProcessOnOutputDataReceived;
                process.ErrorDataReceived += ProcessOnErrorDataReceived;

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }

        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_isTearDown)
                return;

            StepExecution.Current.Comment(e.Data);
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (_isTearDown)
                return;

            StepExecution.Current.Comment(e.Data);
        }
    }
}