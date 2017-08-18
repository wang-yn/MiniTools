using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniTools.Libs.Common
{
    public static class CommandLineHelper
    {
        public static ExecuteResult Execute(string exePath, string argument, bool directOutput = false)
        {
            ExecuteResult result = new ExecuteResult();
            result.CommandLine = exePath + " " + argument;

            try {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = exePath;
                process.StartInfo.Arguments = argument;
                process.StartInfo.ErrorDialog = false;

                process.Start();

                string text;
                StringBuilder sb = new StringBuilder();
                while( (text = process.StandardOutput.ReadLine()) != null ) {
                    text = text.TrimEnd(new char[0]);
                    if( text.Length > 0 ) {
                        if( directOutput )
                            Console.WriteLine(text);
                        else
                            sb.AppendLine(text);
                    }
                }

                result.Output = sb.ToString();
                result.ExitCode = process.ExitCode;
                process.Dispose();
            }
            catch( System.Exception ex ) {
                result.ExitCode = -999;
                result.Exception = ex;
            }
            return result;
        }
    }


    public sealed class ExecuteResult
    {
        public string CommandLine { get; set; }

        public string Output { get; set; }

        public Exception Exception { get; set; }

        public int ExitCode { get; set; }

        public override string ToString()
        {
            return string.Format(@"
调用命令行：
{0}

执行结果：
-----------------------------------------------------------------------------
{1}
-----------------------------------------------------------------------------

",
                this.CommandLine,
                (this.Exception == null ? this.Output : this.Exception.ToString()));
        }
    }
}
