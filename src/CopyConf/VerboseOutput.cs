using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CopyConf
{
    public class VerboseOutput       
    {   
        private readonly TextWriter _stdout;
        private readonly TextWriter _stderr;
        private readonly bool _isVerbose;

        public VerboseOutput() : this(null, false) {}

        public VerboseOutput(CommandLineApplication app, bool isVerbose)
        {
            _stdout = app == null ? Console.Out : app.Out;
            _stderr = app == null ? Console.Error : app.Error;
            _isVerbose = isVerbose;
        }

        public void Debug(string message)
        {
            if (_isVerbose)
            {
                _stdout.WriteLine(message);
            }
        }

        public void Info(string message)
        {
            _stdout.WriteLine(message);
        }
    }
}
