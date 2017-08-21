using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MiniBuild
{
    static class FileSearch
    {
        private static readonly string[] s_searchFolders = {
            // VS 2015
            Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86),@"MSBuild\14.0\Bin"),
            // VS 2017
            Path.Combine(Environment.GetFolderPath( Environment.SpecialFolder.ProgramFilesX86),@"Microsoft Visual Studio\2017\Enterprise\MSBuild\15.0\Bin"),
        };

        /// <summary>
        /// 从一组文件路径中找到有该文件的路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string Search(string filename)
        {
            foreach( string folder in s_searchFolders ) {
                string fullpath = Path.Combine(folder, filename);
                if( File.Exists(fullpath) )
                    return fullpath;
            }

            return string.Empty;
        }
    }
}