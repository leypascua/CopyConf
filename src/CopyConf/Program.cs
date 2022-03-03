using System;
using System.Reflection;

namespace CopyConf
{
    class Program
    {   
        static int Main(string[] args)
        {
            ShowBanner();
            var context = new CommandLineApplicationContext();
            return context.Execute(args);
        }

        private static void ShowBanner()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string assemblyVersion = fileVersion.FileVersion;

            Console.WriteLine(string.Empty);
            Console.WriteLine($"CopyConf version {assemblyVersion}");
            Console.WriteLine("   Written by leypascua. All rights reserved.\r\n");
        }
    }
}
