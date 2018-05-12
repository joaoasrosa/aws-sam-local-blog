using System;
using System.Diagnostics;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;

namespace Lambda.Tests.Acceptance
{
    internal class Container
    {
        private readonly string _apiBehaviour;

        internal Container(
            string apiBehaviour)
        {
            _apiBehaviour = apiBehaviour;
        }

        internal void Start()
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "docker";
                process.StartInfo.Arguments = 
                    $"run -d -i --name testing-api --network testing -p 8000:80 -e API_BEHAVIOUR={_apiBehaviour} joaoasrosa/testing-api";
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
            StepExecution.Current.Comment(e.Data);
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            StepExecution.Current.Comment(e.Data);
        }
    }
}