using Microsoft.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CopyConf
{
    public class CommandLineApplicationContext
    {
        const string SourceDirArg = "sourceDir";

        const string DestDirArg = "destinationDir";
        const int PathNotFoundExitCode = 3;
        const int SuccessfulCompletionExitCode = 0;
        const int BadCommandExitCode = 24;

        private static readonly string _commandName = typeof(Program).Assembly.GetName().Name;
        private static readonly string[] _defaultExtensions = new string[] { ".conf", ".config", ".json", ".yaml", ".yml", ".xml" };
        private readonly CommandOption _rootOnly;
        private readonly CommandOption _supportedExtensions;
        private readonly CommandOption _forceCreate;
        private readonly CommandOption _verbose;
        private readonly CommandArgument _source;
        private readonly CommandArgument _dest;

        public CommandLineApplicationContext()
        {
            var app = new CommandLineApplication
            {
                Name = _commandName,
                Description = "A tool to copy default config files from a specified location to an output directory."
            };

            app.HelpOption("-?|-h|--help");

            _supportedExtensions = app.Option(
                "-e|--extensions",
                $"[OPTIONAL] Supported file extensions delimited by semi-colon. Defaults to: \"{string.Join(";", _defaultExtensions)}\"",
                CommandOptionType.SingleValue);

            _rootOnly = app.Option(
                "-r|--rootonly", 
                "[OPTIONAL] Prevent files in subdirectories from being copied", 
                CommandOptionType.NoValue);

            _forceCreate = app.Option(
                "-f|--force",
                "[OPTIONAL] Force create destination directory when it doesn't exist.",
                CommandOptionType.NoValue);

            _verbose = app.Option(
                "-v|--verbose",
                "[OPTIONAL] Verbose mode.",
                CommandOptionType.NoValue);

            _source = app.Argument(SourceDirArg, "\"/path/to/source\" The path to the directory where default content will be copied from.");            
            _dest = app.Argument(DestDirArg, "\"/path/to/dest\" The path to the directory where files will be copied to");

            app.OnExecute(() => ExecuteApp());

            App = app;
        }

        public CommandLineApplication App { get; }

        public int Execute(string[] args)
        {
            try
            {
                return App.Execute(args);
            }
            catch (CommandParsingException ex)
            {
                App.Error.WriteLine(ex.Message);
                ShowUsage();
                return BadCommandExitCode;
            }
        }

        private int ExecuteApp()
        {
            bool forceCreateDestDirs = _forceCreate.HasValue();
            bool isRecursive = _rootOnly.HasValue() == false;            

            string[] includedFileExtensions = _supportedExtensions.HasValue() ?
                _supportedExtensions.Value().Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries) :
                _defaultExtensions;

            DirectoryInfo sourcePath = GetDirectory(_source.Value, SourceDirArg);
            DirectoryInfo destPath = GetDirectory(_dest.Value, DestDirArg, forceCreate: forceCreateDestDirs);

            var output = new VerboseOutput(App, _verbose.HasValue());
            Helpers.SyncFolders(sourcePath, destPath, includedFileExtensions, isRecursive, output);

            return SuccessfulCompletionExitCode;
        }

        private DirectoryInfo GetDirectory(string path, string context, bool forceCreate = false)
        {
            if (string.IsNullOrEmpty(path))
            {
                App.Error.WriteLine($"Missing required argument: {context} {Environment.NewLine}");
                ShowUsage();
                Environment.Exit(PathNotFoundExitCode);
            }

            var dinfo = new DirectoryInfo(path.Replace("\"", string.Empty));

            if (!dinfo.Exists)
            {
                if (!forceCreate)
                {   
                    App.Error.WriteLine("The {0} path \"{1}\" does not exist or access is denied.", context, path);
                    Environment.Exit(PathNotFoundExitCode);
                }

                dinfo.Create();
            }

            return dinfo;
        }

        private void ShowUsage()
        {
            App.Out.WriteLine("Usage:");
            App.Out.WriteLine($"  {_commandName} {SourceDirArg} {DestDirArg} [--extensions=\"{string.Join(";", _defaultExtensions)}\"] [--rootonly] [--force]");
        }
    }
}
