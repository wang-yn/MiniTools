using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace MiniBuild
{
    class Options
    {
        [Option("debug", DefaultValue = false, HelpText = "以调试模式启动（输出更详细的日志）")]
        public bool DebugMode { get; set; }

        [Option('s', "solution", Required = false, HelpText = "解决方案文件名（含路径）")]
        public string SolutionFile { get; set; }

        [Option('t', "target", DefaultValue = "Build", Required = false, HelpText = "生成目标：compile/rebuild/clean")]
        public string Target { get; set; }

        [Option('c', "configuration", DefaultValue = "debug", Required = false, HelpText = "配置类型：debug/release")]
        public string Configuration { get; set; }

        [Option("silent", DefaultValue = false, HelpText = "以静默模式启动（执行后立即退出）")]
        public bool Slient { get; set; }

        [Option("setup", DefaultValue = false, HelpText = "安装*.sln文件的右键菜单")]
        public bool Setup { get; set; }
    }
}