using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TwosCompany.Helper;
using TwosCompany.ModBG;

namespace TwosCompany {
    public partial class Manifest : IStoryManifest {

        private static string GetHash(string input, SHA256 hash) {
            byte[] bytes = hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (Byte thisByte in bytes)
                builder.Append(thisByte.ToString("x2"));
            return builder.ToString().Substring(0, 8);
        }

        public static void PatchStory(string storyFileName, Dictionary<string, string> loc) {
            if (Manifest.Instance.ModRootFolder == null)
                throw new Exception("Root Folder not set");

            Type[] bgs = Assembly.GetExecutingAssembly().GetTypes()
              .Where(t => String.Equals(t.Namespace, "TwosCompany.ModBG"))
              .ToArray();

            foreach (Type bg in bgs)
                DB.backgrounds.Add("TwosCompany.ModBG" + "." + bg.Name, bg);

            MethodInfo[] commands = typeof(StoryCommandsTC).GetMethods();
            foreach (MethodInfo command in commands) {
                if (command.Name.ToLower().Contains("mem"))
                    DB.storyCommands.Add(command.Name, command);
            }


            MethodInfo[] memChoices = typeof(StoryChoicesTC).GetMethods();
            foreach (MethodInfo command in memChoices) {
                if (command.Name.ToLower().Contains("mem"))
                    DB.eventChoiceFns.Add(command.Name, command);
            }

            DB.mapBgs.Add("zone_mezzmapmem_first",
                (Spr)(Manifest.Sprites["MapBGJost"].Id ?? throw new Exception("missing mapbg"))
            );

            Story parseStory = Mutil.LoadJsonFile<Story>(Path.Combine(Manifest.Instance.ModRootFolder.FullName, "story", Path.GetFileName(storyFileName + ".json")));
            SHA256 hash = SHA256.Create();
            foreach (string key in parseStory.all.Keys) {
                int counter = 0;
                // Manifest.Instance.Logger!.LogInformation("key: " + key);
                if (DB.story.all.ContainsKey(key)) {
                    for (int i = 0; i < DB.story.all[key].lines.Count && i < parseStory.all[key].lines.Count; i++) {
                        Instruction line = DB.story.all[key].lines[i];
                        if (line is SaySwitch saySwitch && parseStory.all[key].lines[i] is SaySwitch addLines) {
                            foreach (Say sayLine in addLines.lines) {
                                string newHash = GetHash(key + ":" + counter + ":mezz_" + sayLine.who, hash);
                                DB.currentLocale.strings.Add(key + ":" + newHash, loc[key + ":" + counter + ":mezz_" + sayLine.who]);
                                if (ManifHelper.charStoryNames.ContainsKey(sayLine.who))
                                    sayLine.who = ManifHelper.charStoryNames[sayLine.who];
                                sayLine.hash = newHash;
                                saySwitch.lines.Add(sayLine);
                            }
                        }
                    }
                    counter++;
                }
            }
        }

        private void LoadStory(string storyFileName, Dictionary<string, string> loc, Dictionary<string, string> whos, IStoryRegistry? storyRegistry) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            Story parseStory = Mutil.LoadJsonFile<Story>(Path.Combine(ModRootFolder.FullName, "story", Path.GetFileName(storyFileName + ".json")));
            List<String> hashes = new List<String>();
            List<String> whats = new List<String>();
            SHA256 hash = SHA256.Create();

            foreach (string key in parseStory.all.Keys) {
                string keyName = key;
                foreach (string charKey in ManifHelper.charStoryNames.Keys) {
                    if (keyName.Contains(charKey + "ID")) {
                        keyName = Regex.Replace(keyName, charKey + "ID", "" + ManifHelper.GetDeckId(charKey));
                        break;
                    }
                }
                if (whos.ContainsKey(key))
                    parseStory.all[key].whoDidThat = ManifHelper.GetDeck(whos[key]);

                if (parseStory.all[key].allPresent != null) {
                    foreach (String crew in parseStory.all[key].allPresent!.ToList()) {
                        if (ManifHelper.charStoryNames.ContainsKey(crew)) {
                            parseStory.all[key].allPresent!.Remove(crew);
                            parseStory.all[key].allPresent!.Add(ManifHelper.charStoryNames[crew]);
                        }
                    }
                }
                if (parseStory.all[key].nonePresent != null) {
                    foreach (String crew in parseStory.all[key].nonePresent!.ToList()) {
                        if (ManifHelper.charStoryNames.ContainsKey(crew)) {
                            parseStory.all[key].nonePresent!.Remove(crew);
                            parseStory.all[key].nonePresent!.Add(ManifHelper.charStoryNames[crew]);
                        }
                    }
                }
                int current = 0;
                foreach (Instruction line in parseStory.all[key].lines) {
                    if (line is Say sayLine) {
                        if (ManifHelper.charStoryNames.ContainsKey(sayLine.who))
                            sayLine.who = ManifHelper.charStoryNames[sayLine.who];
                        string newHash = GetHash(sayLine.who + loc[key + ":" + current], hash);
                        whats.Add(key + ":" + current);
                        sayLine.hash = newHash;
                        hashes.Add(newHash);
                        current++;
                    }
                    else if (line is SaySwitch switchLines) {
                        int switchCounter = 0;
                        foreach (Say switchLine in switchLines.lines) {
                            if (ManifHelper.charStoryNames.ContainsKey(switchLine.who))
                                switchLine.who = ManifHelper.charStoryNames[switchLine.who];
                            string newHash = GetHash(switchLine.who + loc[key + ":" + current + (char)(switchCounter + 97)], hash);
                            whats.Add(key + ":" + current + (char) (switchCounter + 97));
                            switchLine.hash = newHash;
                            hashes.Add(newHash);
                            switchCounter++;
                        }
                        current++;
                    } else if (line is TitleCard card) {
                        if (!(card.empty??false)) {
                            whats.Add(card.hash);
                            hashes.Add(card.hash);
                        }
                    }
                }
                if (storyRegistry != null) {
                    ExternalStory newStory = new ExternalStory(keyName, parseStory.all[key], parseStory.all[key].lines);
                    for (int i = 0; i < hashes.Count; i++) {
                        if (!loc.ContainsKey(whats[i]))
                            throw new Exception("key not found in loc: " + whats[i]);
                        newStory.AddLocalisation(hashes[i], loc[whats[i]]);
                    }

                    storyRegistry.RegisterStory(newStory);
                } else {

                }

                hashes.Clear();
                whats.Clear();
            }
        }
        public void LoadManifest(IStoryRegistry storyRegistry) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            Dictionary<string, string> loc = Mutil.LoadJsonFile<Dictionary<string, string>>(Path.Combine(ModRootFolder.FullName, "locales", Path.GetFileName("en.json")));
            Dictionary<string, string> whos = Mutil.LoadJsonFile<Dictionary<string, string>>(Path.Combine(ModRootFolder.FullName, "story", Path.GetFileName("whos.json")));
            LoadStory("story_nola", loc, whos, storyRegistry);
            LoadStory("story_isabelle", loc, whos, storyRegistry);
            LoadStory("story_ilya", loc, whos, storyRegistry);
            LoadStory("story_jost", loc, whos, storyRegistry);
            LoadStory("story_gauss", loc, whos, storyRegistry);
            LoadStory("story_sorrel", loc, whos, storyRegistry);
        }
    }
}
