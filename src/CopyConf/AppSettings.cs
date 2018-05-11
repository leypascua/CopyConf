using System;

namespace CopyConf
{
    public class AppSettings
    {
        const string CopyConfIncludedFilesKey = "CopyConf:Include";
        const string DefaultSupportedFiles = ".conf;.config;.json;.yaml;.yml;.ini;";

        static AppSettings()
        {
            string includeVal = (Environment.GetEnvironmentVariable(CopyConfIncludedFilesKey) ?? string.Empty)
                .Trim();

            Current = new AppSettings
            {
                Include = string.IsNullOrEmpty(includeVal) ? DefaultSupportedFiles : includeVal
            };
        }

        public static AppSettings Current { get; private set; }

        public string Include { get; internal set; }
        
    }
}
