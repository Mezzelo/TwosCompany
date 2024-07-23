
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;

namespace TwosCompany {

    public enum SettingsDifficulty
    {
        Normal, Hard, Harder, Hardest
    }
    public class ModSettings {
        public bool unlockAll = false;
        public bool unlockNola = false;
        public bool unlockIsa = false;
        public bool unlockIlya = false;
        public bool unlockJost = false;
        public bool unlockGauss = false;
        public bool unlockSorrel = false;
        public SettingsDifficulty memoryDifficulty = SettingsDifficulty.Hardest;
        public bool memHistory = false;


        // antiquated - using nickel helper now.
        private const string fileName = "Mezz.TwosCompany.json";
        public void Save() {
            string text = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CobaltCore", "Nickel", "ModStorage", 
                fileName);
            bool flag = true;
            string text2 = flag ? (text + ".tmp") : text;
            using (FileStream fileStream = File.Open(text2, FileMode.Create)) {
                using StreamWriter textWriter = new StreamWriter(fileStream);
                JSONSettings.serializer.Serialize(textWriter, this);
            }
            if (flag)
                File.Move(text2, text, overwrite: true);
        }

        public static ModSettings LoadOrNew() {
            ModSettings? settings = null;
            try {
                using FileStream fileStream = File.OpenRead(
                    Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CobaltCore", "Nickel", "ModStorage",
                    fileName)
                );
                using StreamReader reader = new StreamReader(fileStream);
                using JsonTextReader reader2 = new JsonTextReader(reader);
                settings = JSONSettings.serializer.Deserialize<ModSettings>(reader2);
            }
            catch {
            }
            if (settings == null) {
                settings = new ModSettings();
            }
            return settings;
        }
    }

}
