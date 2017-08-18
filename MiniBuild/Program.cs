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
            var options = new Options();
            if( CommandLine.Parser.Default.ParseArgumentsStrict(args, options) == false ) {
                return;
            }

            //Console.WriteLine(options.SolutionFile);
            //Console.WriteLine(options.Configuration);
            //Console.WriteLine(options.Target);
            //Console.WriteLine(options.DebugMode);

            //Console.WriteLine(new string('-', 20));

            //Console.WriteLine("System Path Is:");
            //foreach( var p in DirectoryHelper.GetSystemPath() )
            //    Console.WriteLine(p);

            var msbuildpath = FileSearch.GetFullPath("msbuild.exe");
            //Console.WriteLine(msbuildpath);

            var pms = new List<string>();
            pms.Add("/t:" + options.Target);
            pms.Add("/p:Configuration=" + options.Configuration);
            pms.Add(options.SolutionFile);

            var paramString = string.Join(" ", pms);

            var result = CommandLineHelper.Execute(msbuildpath, paramString, true);
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
    }

    static class FileSearch
    {
        private static string[] s_searchFolders = new[] {
            Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86),"MSBuild\\14.0\\Bin"),
            Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86),"Microsoft Visual Studio\\2017\\Enterprise\\MSBuild\\15.0\\Bin"),
        };

        /// <summary>
        /// 从一组文件路径中找到有该文件的路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFullPath(string filename)
        {
            foreach( var folder in s_searchFolders ) {
                var fullpath = Path.Combine(folder, filename);
                if( File.Exists(fullpath) )
                    return fullpath;
            }

            return string.Empty;
        }
    }
}
