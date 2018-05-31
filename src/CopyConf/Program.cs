using System;
using System.IO;
using System.Linq;

namespace CopyConf
{
    class Program
    {
        const int PathNotFoundExitCode = 3;
        const int SuccessfulCompletionExitCode = 0;
        const int SourcePathArgsIndex = 0;
        const int DestinationPathArgsIndex = 1;        

        static int Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                return ShowUsage();
            }

            return Execute(args);
        }

        private static int ShowUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  {0} /path/to/source /path/to/destination [--rootonly]", typeof(Program).Assembly.GetName().Name);
            Environment.Exit(PathNotFoundExitCode);

            return PathNotFoundExitCode;
        }

        private static int Execute(string[] args)
        {
            DirectoryInfo sourcePath = GetDirectory(args[SourcePathArgsIndex], "source");
            DirectoryInfo destinationPath = GetDirectory(args[DestinationPathArgsIndex], "destination", forceCreate: true);
            bool isRecursive = true;

            if (args.Length == 3)
            {  
                string rootOnlySwitch = args.Last();

                if (rootOnlySwitch.ToLowerInvariant() != "--rootonly")
                {
                    return ShowUsage();
                }

                isRecursive = false;                
            }

            string[] includedFileExtensions = (AppSettings.Current.Include ?? string.Empty)
                .Split(new[] {';', ' ', ','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(ext => ext.Trim())
                .ToArray();

            Helpers.SyncFolders(sourcePath, destinationPath, includedFileExtensions, isRecursive);

            return SuccessfulCompletionExitCode;
        }

        private static DirectoryInfo GetDirectory(string path, string context, bool forceCreate = false)
        {
            var dinfo = new DirectoryInfo(path.Replace("\"", string.Empty));

            if (!dinfo.Exists)
            {
                if (!forceCreate)
                {
                    Console.WriteLine("The {0} path {1} does not exist or access is denied.", context, path);
                    Environment.Exit(PathNotFoundExitCode);
                }

                dinfo.Create();    
            }

            return dinfo;
        }
    }
}
