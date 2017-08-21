using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniTools.Libs.Common;

namespace MiniBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Options options = new Options();
            if( CommandLine.Parser.Default.ParseArgumentsStrict(args, options) == false ) {
                return;
            }

            // 安装模式
            if( options.Setup ) {
                Setup();
                Console.WriteLine("右键菜单安装成功");
                return;
            }

            if( string.IsNullOrEmpty(options.SolutionFile) ) {
                Console.WriteLine("解决方案文件名不能为空。");
                Console.WriteLine(CommandLine.Text.HelpText.AutoBuild(options).ToString());
                return;
            }

            string msbuildpath = FileSearch.Search("msbuild.exe");

            List<string> pms = new List<string> {
                "/t:" + options.Target,
                "/p:Configuration=" + options.Configuration,
                options.SolutionFile
            };

            string paramString = string.Join(" ", pms);

            ExecuteResult result = CommandLineHelper.Execute(msbuildpath, paramString, true);
            Console.WriteLine(result.CommandLine);

            if( result.ExitCode == 0 ) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"编译成功 {options.SolutionFile}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else {
                Console.WriteLine(result.Output);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"编译失败 {options.SolutionFile}");
                Console.ForegroundColor = ConsoleColor.White;
            }

            if( options.Slient == false )
                Console.ReadLine();
        }

        private static void Setup()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MiniBuild.exe");
            string cmd = path + " -s \"%1\"";
            RegistryHelper.Write(@"VisualStudio.Launcher.sln\Shell\MiniBuild", null, "一键编译");
            RegistryHelper.Write(@"VisualStudio.Launcher.sln\Shell\MiniBuild\Command", null, cmd);
            RegistryHelper.Write(@"VisualStudio.sln.14.0\Shell\MiniBuild", null, "一键编译");
            RegistryHelper.Write(@"VisualStudio.sln.14.0\Shell\MiniBuild\Command", null, cmd);
        }
    }


}
