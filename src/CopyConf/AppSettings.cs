using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;

namespace CopyConf
{
    public class AppSettings
    {
        static AppSettings()
        {
            Current = CreateInstance(ConfigurationManager.AppSettings);
        }

        public static AppSettings Current { get; private set; }

        public string Include { get; private set; }

        public static AppSettings CreateInstance(NameValueCollection settings)
        {
            return new AppSettings
            {
                Include = settings["Include"] ?? ".json;.config;.conf;.cfg;.yaml"
            };
        }
    }
}
