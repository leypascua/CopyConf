using System;

namespace CopyConf
{
    class Program
    {   
        static int Main(string[] args)
        {
            var context = new CommandLineApplicationContext();
            return context.Execute(args);
        }
    }
}
