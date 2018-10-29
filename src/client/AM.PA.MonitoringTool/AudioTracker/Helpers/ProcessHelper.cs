using Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioTracker.Helpers.ProcessHelper
{

    public static class ProcessHelper
    {
        public static ProcessResult ExecuteShellCommand(string command, string arguments, int timeout)
        {
            var result = new ProcessResult();

            using (var process = new Process())
            {
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                var outputBuilder = new StringBuilder();
                var errorBuilder = new StringBuilder();

                using (var outputCloseEvent = new System.Threading.AutoResetEvent(false))
                using (var errorCloseEvent = new System.Threading.AutoResetEvent(false))
                {
                    var copyOutputCloseEvent = outputCloseEvent;

                    process.OutputDataReceived += (s, e) =>
                    {
                        // Output stream is closed (process completed)
                        if (string.IsNullOrEmpty(e.Data))
                        {
                            copyOutputCloseEvent.Set();
                        }
                        else
                        {
                            outputBuilder.AppendLine(e.Data);
                        }
                    };

                    var copyErrorCloseEvent = errorCloseEvent;

                    process.ErrorDataReceived += (s, e) =>
                    {
                        // Error stream is closed (process completed)
                        if (string.IsNullOrEmpty(e.Data))
                        {
                            copyErrorCloseEvent.Set();
                        }
                        else
                        {
                            errorBuilder.AppendLine(e.Data);
                        }
                    };

                    bool isStarted;

                    try
                    {
                        isStarted = process.Start();
                    }
                    catch (Exception error)
                    {
                        Logger.WriteToLogFile(error);
                        result.Completed = true;
                        result.ExitCode = -1;
                        result.Output = error.Message;

                        isStarted = false;
                    }

                    if (isStarted)
                    {
                        // Read the output stream first and then wait because deadlocks are possible
                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        if (process.WaitForExit(timeout)
                            && outputCloseEvent.WaitOne(timeout)
                            && errorCloseEvent.WaitOne(timeout))
                        {
                            result.Completed = true;
                            result.ExitCode = process.ExitCode;

                            if (process.ExitCode != 0)
                            {
                                result.Output = $"{outputBuilder}{errorBuilder}";
                            }
                        }
                        else
                        {
                            try
                            {
                                // Kill hung process
                                process.Kill();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            return result;
        }


        public struct ProcessResult
        {
            public bool Completed;
            public int? ExitCode;
            public string Output;
        }
    }

}
