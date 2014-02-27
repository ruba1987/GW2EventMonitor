using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance
{
    public class SettingsManager
    {
        private readonly string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "GW2EventMon");
        private readonly string basicSettingsFile = "BasicSettings.json";
        private readonly string eventSettingsFile = "EventSettings.json";
        public SettingsManager()
        {
            if (!Directory.Exists(settingsPath))
                Directory.CreateDirectory(settingsPath);
            if (!File.Exists(Path.Combine(settingsPath, basicSettingsFile)))
                File.Create(Path.Combine(settingsPath, basicSettingsFile));

            if (!Directory.Exists(settingsPath))
                Directory.CreateDirectory(settingsPath);
            if (!File.Exists(Path.Combine(settingsPath, eventSettingsFile)))
                File.Create(Path.Combine(settingsPath, eventSettingsFile));
        }

        public ISettings GetSettings(SettingType s)
        {
            switch (s)
            {
                case SettingType.Baisc:
                    try
                    {
                        return JsonConvert.DeserializeObject<BasicSettings>(File.ReadAllText(Path.Combine(settingsPath, basicSettingsFile))) ?? new BasicSettings();
                    }
                    catch
                    {
                        return new BasicSettings();
                    }
                case SettingType.Event:
                    try
                    {
                        return JsonConvert.DeserializeObject<EventSettings>(File.ReadAllText(Path.Combine(settingsPath, eventSettingsFile))) ?? new EventSettings();
                    }
                    catch
                    {
                        return new EventSettings();
                    }
                default:
                    throw new ArgumentException("Unsupported settings type");
            }
        }

        public void Save(ISettings s)
        {
            string filePath;
            if (s is BasicSettings)
                filePath = Path.Combine(settingsPath, basicSettingsFile);
            else if(s is EventSettings)
                filePath = Path.Combine(settingsPath, eventSettingsFile);
            else
                throw new ArgumentException("Unsupported settings type");

            File.WriteAllText(filePath, JsonConvert.SerializeObject(s, Formatting.Indented));
        }
    }
}
