using System;
using System.Diagnostics;
using System.Threading;
using LightBDD.Framework;
using LightBDD.Framework.Commenting;

namespace Lambda.Tests.Acceptance
{
    internal class ApiGateway
    {
        private readonly string _path;
        private Process _process;

        internal ApiGateway(
            string path)
        {
            _path = path;
        }

        internal void Start()
        {
            _process = new Process
            {
                StartInfo =
                {
                    FileName = "sam",
                    Arguments = "local start-api --docker-network testing",
                    WorkingDirectory = _path,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            
            _process.OutputDataReceived += ProcessOnOutputDataReceived;
            _process.ErrorDataReceived += ProcessOnErrorDataReceived;
          
            _process.Start();
            _process.BeginOutputReadLine();
            _process.BeginErrorReadLine();
            
            Thread.Sleep(3000);
        }

        internal void Stop()
        {
            _process.Kill();
            _process.Close();
            _process.Dispose();
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