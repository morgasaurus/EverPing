using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace EverPingUi
{
    public static class AppMethods
    {
        public const string ConfigFileName = "config.json";

        public static Settings DefaultSettings = new Settings() { Host = "www.google.com", Timeout = 3000, Bytes = 32 };

        public static Settings ReadConfig()
        {
            try
            {
                if (!File.Exists(ConfigFileName))
                {
                    return DefaultSettings;
                }

                return JsonConvert.DeserializeObject<Settings>(File.ReadAllText(ConfigFileName));
            }
            catch
            {
                return DefaultSettings;
            }
        }

        public static void SaveConfig(Settings settings)
        {
            File.WriteAllText(ConfigFileName, JsonConvert.SerializeObject(settings, Formatting.Indented));
        }
    }
}
