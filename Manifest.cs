using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using TwosCompany.Helper;
using TwosCompany.Cards.Nola;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;
using TwosCompany.Cards.Gauss;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using System.Reflection;
using TwosCompany.Artifacts;
using Microsoft.Xna.Framework.Graphics;
using TwosCompany.ExternalAPI;

namespace TwosCompany {
    public partial class Manifest : ISpriteManifest, ICardManifest, IDeckManifest, ICharacterManifest, IAnimationManifest,
        IGlossaryManifest, IStatusManifest, 
        ICustomEventManifest, IArtifactManifest, IModManifest {
        public DirectoryInfo? ModRootFolder { get; set; }
        public DirectoryInfo? GameRootFolder { get; set; }
        internal static Manifest Instance { get; private set; } = null!;
        internal static APIImplementation Api { get; private set; } = null!;
        public static IMoreDifficultiesApi? MoreDifficultiesApi = null;

        public string Name { get; init; } = "Mezz.TwosCompany";
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[]
        {
            new DependencyEntry<IModManifest>("TheJazMaster.MoreDifficulties", true)
        };
        public ILogger? Logger { get; set; }

        public static Dictionary<string, ExternalSprite> Sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalAnimation> Animations = new Dictionary<string, ExternalAnimation>();

        public static Dictionary<string, ExternalCard>? Cards = new Dictionary<string, ExternalCard>();

        public static Dictionary<string, ExternalArtifact> Artifacts = new Dictionary<string, ExternalArtifact>();

        public static ExternalCharacter? NolaCharacter { get; private set; }
        public static ExternalDeck? NolaDeck { get; private set; }
        public static System.Drawing.Color NolaColor = System.Drawing.Color.FromArgb(23, 175, 198); // 17AFC6
        public static String NolaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", NolaColor.R, NolaColor.G, NolaColor.B.ToString("X2"));
        public static string[] nolaEmotes = new String[] {
            "mini", "neutral", "gameover", "crystallized", "nap", "angry", "annoyed", "getreal", "happy", "smug", "squint", "vengeful", 
        };

        public static ExternalCharacter? IsabelleCharacter { get; private set; }
        public static ExternalDeck? IsabelleDeck { get; private set; }
        public static System.Drawing.Color IsabelleColor = System.Drawing.Color.FromArgb(47, 72, 183); // 2F48B7
        public static String IsaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", IsabelleColor.R, IsabelleColor.G, IsabelleColor.B);
        public static string[] isabelleEmotes = new String[] {
            "mini", "neutral", "gameover", "crystallized", "nap", "angry", "forlorn", "getreal", "glare", "happy", "swordhappy", "shocked", "snide", "squint", "swordsquint", 
        };

        public static ExternalCharacter? IlyaCharacter { get; private set; }
        public static ExternalDeck? IlyaDeck { get; private set; }
        public static System.Drawing.Color IlyaColor = System.Drawing.Color.FromArgb(188, 84, 116); // BC5474
        public static String IlyaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", IlyaColor.R, IlyaColor.G, IlyaColor.B.ToString("X2"));
        public static string[] ilyaEmotes = new String[] {
            "mini", "neutral", "gameover", "crystallized", "nap", "bashful", "blush", "forlorn", "happy", "intense", "shocked", "side", "squint", 
        };

        public static ExternalCharacter? JostCharacter { get; private set; }
        public static ExternalDeck? JostDeck { get; private set; }
        public static System.Drawing.Color JostColor = System.Drawing.Color.FromArgb(135, 178, 207); // 87B2CF
        public static String JostColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", JostColor.R, JostColor.G, JostColor.B);
        public static string[] jostEmotes = new String[] {
            "mini", "neutral", "gameover", "squint", "crystallized", "annoyed", "forlorn", "happy", "nap", "swordannoyed", "swordhappy", "swordneutral", "swordsquint", 
        };

        public static ExternalCharacter? GaussCharacter { get; private set; }
        public static ExternalDeck? GaussDeck { get; private set; }
        public static System.Drawing.Color GaussColor = System.Drawing.Color.FromArgb(180, 73, 190); // B449BE
        public static String GaussColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", GaussColor.R, GaussColor.G, GaussColor.B.ToString("X2"));
        public static System.Drawing.Color ChainColor = System.Drawing.Color.FromArgb(0, 229, 255); // 00E5FF
        public static String ChainColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", ChainColor.R, ChainColor.G, ChainColor.B.ToString("X2"));
        public static string[] gaussEmotes = new String[] {
            "mini", "neutral", "gameover", "squint", "crystallized", "angry", "datapad", "forlorn", "happy", "nap", "serious", "side", "vengeful", "getreal",
        };

        private void addCharSprite(string charName, string emote, string subfolder, ISpriteRegistry artReg) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            int i = 0;
            string path = Path.Combine(ModRootFolder.FullName, "Sprites", subfolder, "mezz_" + charName, Path.GetFileName("mezz_" + charName + "_" + emote + "_" + i + ".png"));
            String spriteName = string.Concat(charName[0].ToString().ToUpper(), charName.AsSpan(1)) + string.Concat(emote[0].ToString().ToUpper(), emote.AsSpan(1));
            while (File.Exists(path)) {
                Sprites.Add(spriteName + i, new ExternalSprite("Mezz.TwosCompany." + spriteName + i, new FileInfo(path)));
                artReg.RegisterArt(Sprites[spriteName + i]);
                i++;
                path = Path.Combine(ModRootFolder.FullName, "Sprites", subfolder, "mezz_" + charName, Path.GetFileName("mezz_" + charName + "_" + emote + "_" + i + ".png"));
            }
        }
        
        private void addSprite(string name, string spriteName, string subfolder, ISpriteRegistry artReg) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            Sprites.Add(name, new ExternalSprite("Mezz.TwosCompany." + name, new FileInfo(
                Path.Combine(ModRootFolder.FullName, "Sprites", subfolder, Path.GetFileName("mezz_" + spriteName + ".png"))
                )));
            artReg.RegisterArt(Sprites[name]);
        }
        private void addSprite(string name, string subfolder, ISpriteRegistry artReg) {
            addSprite(name, name, subfolder, artReg);
        }

        private void addEmoteAnim(string charName, string emote, IAnimationRegistry animReg, ExternalDeck deck) {
            String spriteName = string.Concat(charName[0].ToString().ToUpper(), charName.AsSpan(1)) + string.Concat(emote[0].ToString().ToUpper(), emote.AsSpan(1));
            if (!Sprites.ContainsKey(spriteName + "0"))
                throw new Exception("missing sprite: " + spriteName);
            List<ExternalSprite> thisAnimSprites = new List<ExternalSprite>();
            int i = 0;
            while (Sprites.ContainsKey(spriteName + i)) {
                thisAnimSprites.Add(Sprites[spriteName + i]);
                i++;
            }
            Animations.Add(spriteName + "Anim", new ExternalAnimation("Mezz.TwosCompany." + spriteName, deck, emote, false, thisAnimSprites));
            animReg.RegisterAnimation(Animations[spriteName + "Anim"]);

        }

        void ISpriteManifest.LoadManifest(ISpriteRegistry artReg) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            // cardSprites
            ManifHelper.DefineCardSprites(ModRootFolder, Sprites);
            foreach (String cardName in ManifHelper.cardNames.Keys) {
                if (!ManifHelper.defaultArtCards.Contains(cardName))
                    artReg.RegisterArt(Sprites[(cardName + "CardSprite")]);
            }
            foreach (String cardName in ManifHelper.hasFlipSprite)
                artReg.RegisterArt(Sprites[(cardName + "CardSpriteFlip")]);

            foreach (String cardName in ManifHelper.jostQuads.Keys) {
                if (ManifHelper.jostQuads[cardName].Equals("unique")) {
                    artReg.RegisterArt(Sprites[(cardName + "CardSpriteFlip")]);
                    artReg.RegisterArt(Sprites[(cardName + "CardSpriteBoth")]);
                    artReg.RegisterArt(Sprites[(cardName + "CardSpriteNeither")]);
                }
            }

            // midrow
            addSprite("DroneConduit", "conduit", "drones", artReg);
            addSprite("DroneConduitKinetic", "conduitKinetic", "drones", artReg);
            addSprite("DroneConduitShield", "conduitShield", "drones", artReg);
            addSprite("DroneConduitShieldDisabled", "conduitShieldDisabled", "drones", artReg);
            addSprite("DroneConduitFeedback", "conduitFeedback", "drones", artReg);
            addSprite("DroneConduitFeedbackDisabled", "conduitFeedbackDisabled", "drones", artReg);

            addSprite("IconConduit", "conduit", "icons", artReg);
            addSprite("IconConduitShield", "conduitShield", "icons", artReg);
            addSprite("IconConduitKinetic", "conduitKinetic", "icons", artReg);
            addSprite("IconConduitFeedback", "conduitFeedback", "icons", artReg);

            // hint/cardaction icons
            addSprite("IconEnergyPerCard", "energyPerCard", "icons", artReg);
            addSprite("IconEnergyPerCardPerma", "energyPerCardPerma", "icons", artReg);
            addSprite("IconEnergyPerAttack", "energyPerAttack", "icons", artReg);
            addSprite("IconEnergyPerPlay", "energyPerPlay", "icons", artReg);
            addSprite("IconRaiseCostHint", "energyPerPlay", "icons", artReg);
            addSprite("IconLowerPerPlay", "lowerPerPlay", "icons", artReg);
            addSprite("IconLowerCostHint", "lowerPerPlay", "icons", artReg);
            addSprite("IconTurnIncreaseCost", "turnIncreaseCost", "icons", artReg);
            addSprite("IconAllIncrease", "allIncrease", "icons", artReg);
            addSprite("IconAllIncreaseCombat", "allIncreaseCombat", "icons", artReg);
            addSprite("IconShieldCost", "shieldCost", "icons", artReg);
            addSprite("IconShieldCostOff", "shieldCostOff", "icons", artReg);
            addSprite("IconEvadeCost", "evadeCost", "icons", artReg);
            addSprite("IconEvadeCostOff", "evadeCostOff", "icons", artReg);
            addSprite("IconHeatCost", "heatCost", "icons", artReg);
            addSprite("IconHeatCostOff", "heatCostOff", "icons", artReg);
            addSprite("IconDefensiveStanceCost", "defensiveStanceCost", "icons", artReg);
            addSprite("IconDefensiveStanceCostOff", "defensiveStanceCostOff", "icons", artReg);
            addSprite("IconOffensiveStanceCost", "offensiveStanceCost", "icons", artReg);
            addSprite("IconOffensiveStanceCostOff", "offensiveStanceCostOff", "icons", artReg);
            addSprite("IconPointDefenseLeft", "pdLeft", "icons", artReg);
            addSprite("IconPointDefense", "pdRight", "icons", artReg);
            addSprite("IconCallAndResponseHint", "callAndResponseHint", "icons", artReg);
            addSprite("IconDisguisedHint", "disguisedHint", "icons", artReg);
            addSprite("IconDisguisedPermaHint", "disguisedPermaHint", "icons", artReg);
            addSprite("IconStanceCard", "stanceCard", "icons", artReg);
            addSprite("IconChainLightning", "chainLightning", "icons", artReg);
            addSprite("IconConductorField", "conductorField", "icons", artReg);

            // status icons
            addSprite("IconTempStrafe", "tempStrafe", "icons", artReg);
            addSprite("IconMobileDefense", "mobileDefense", "icons", artReg);
            addSprite("IconUncannyEvasion", "uncannyEvasion", "icons", artReg);
            addSprite("IconOnslaught", "onslaught", "icons", artReg);
            // addSprite("IconRepeat", "repeat", "icons", artReg);
            // addSprite("IconThreepeat", "threepeat", "icons", artReg);
            addSprite("IconDominance", "dominance", "icons", artReg);
            addSprite("IconFalseOpening", "falseOpening", "icons", artReg);
            addSprite("IconFalseOpeningB", "falseOpeningB", "icons", artReg);
            addSprite("IconEnflamed", "enflamed", "icons", artReg);
            addSprite("IconOffensiveStance", "offensiveStance", "icons", artReg);
            addSprite("IconDefensiveStance", "defensiveStance", "icons", artReg);
            addSprite("IconStandFirm", "standFirm", "icons", artReg);
            addSprite("IconBattleTempo", "battleTempo", "icons", artReg);
            addSprite("IconFortress", "fortress", "icons", artReg);
            addSprite("IconFootwork", "footwork", "icons", artReg);
            addSprite("IconDistantStrike", "distantStrike", "icons", artReg);
            addSprite("IconElectrocuteCharge", "electrocuteCharge", "icons", artReg);
            addSprite("IconElectrocuteChargeSpent", "electrocuteChargeSpent", "icons", artReg);
            // addSprite("IconElectrocuteSource", "electrocuteSource", "icons", artReg);
            addSprite("IconAutocurrent", "autocurrent", "icons", artReg);
            addSprite("IconHyperspaceStorm", "hyperspaceStorm", "icons", artReg);
            addSprite("IconHyperspaceStormA", "hyperspaceStormA", "icons", artReg);
            addSprite("IconHyperspaceStormB", "hyperspaceStormB", "icons", artReg);
            // chars
            addSprite("NolaFrame", "char_nola", "panels", artReg);
            addSprite("IsabelleFrame", "char_isabelle", "panels", artReg);
            addSprite("IlyaFrame", "char_ilya", "panels", artReg);
            addSprite("JostFrame", "char_jost", "panels", artReg);
            addSprite("GaussFrame", "char_gauss", "panels", artReg);

            addSprite("NolaDeckFrame", "border_nola", "cardshared", artReg);
            addSprite("IsabelleDeckFrame", "border_isabelle", "cardshared", artReg);
            addSprite("IlyaDeckFrame", "border_ilya", "cardshared", artReg);
            addSprite("JostDeckFrame", "border_jost", "cardshared", artReg);
            addSprite("GaussDeckFrame", "border_gauss", "cardshared", artReg);

            addSprite("NolaDefaultCardSprite", "default_nola", "cards", artReg);
            addSprite("IsabelleDefaultCardSprite", "default_isabelle", "cards", artReg);
            addSprite("IlyaDefaultCardSprite", "default_ilya", "cards", artReg);
            addSprite("JostDefaultCardSprite", "default_jost", "cards", artReg);
            addSprite("JostDefaultCardSpriteFlip", "default_jost_flip", "cards", artReg);
            addSprite("JostDefaultCardSpriteBoth", "default_jost_both", "cards", artReg);
            addSprite("JostDefaultCardSpriteNeither", "default_jost_neither", "cards", artReg);
            addSprite("JostDefaultCardSpriteUnsided", "default_jost_unsided", "cards", artReg);
            addSprite("JostDefaultCardSpriteUp1", "default_jost_up1", "cards", artReg);
            addSprite("JostDefaultCardSpriteUp1Flip", "default_jost_up1_flip", "cards", artReg);
            addSprite("JostDefaultCardSpriteUp1Both", "default_jost_up1_both", "cards", artReg);
            addSprite("JostDefaultCardSpriteUp1Neither", "default_jost_up1_neither", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown1", "default_jost_down1", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown1Flip", "default_jost_down1_flip", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown1Both", "default_jost_down1_both", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown1Neither", "default_jost_down1_neither", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown2", "default_jost_down2", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown2Flip", "default_jost_down2_flip", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown2Both", "default_jost_down2_both", "cards", artReg);
            addSprite("JostDefaultCardSpriteDown2Neither", "default_jost_down2_neither", "cards", artReg);
            addSprite("GaussDefaultCardSprite", "default_gauss", "cards", artReg);

            addSprite("NolaFullbodySprite", "nola_end", "fullchars", artReg);
            addSprite("IsabelleFullbodySprite", "isabelle_end", "fullchars", artReg);
            addSprite("IlyaFullbodySprite", "ilya_end", "fullchars", artReg);
            addSprite("GaussFullbodySprite", "gauss_end", "fullchars", artReg);

            addSprite("CoreSceneNolaPeri", "core_scene_nola_peri", "bg", artReg);
            addSprite("ShipIsabelle", "ship_isabelle", "bg", artReg);

            addSprite("PerlinNoise", "perlinRoughNormalized", "fx", artReg);

            foreach (String emote in nolaEmotes)
                addCharSprite("nola", emote, "characters", artReg);

            foreach (String emote in isabelleEmotes)
                addCharSprite("isabelle", emote, "characters", artReg);

            foreach (String emote in ilyaEmotes)
                addCharSprite("ilya", emote, "characters", artReg);

            foreach (String emote in jostEmotes)
                addCharSprite("jost", emote, "characters", artReg);

            foreach (String emote in gaussEmotes)
                addCharSprite("gauss", emote, "characters", artReg);

            // artifact icons
            foreach (String artifact in ManifArtifactHelper.artifactNames.Keys)
                addSprite("Icon" + artifact, string.Concat(artifact[0].ToString().ToLower(), artifact.AsSpan(1)), "artifacts", artReg);

            // variable artifact icons
            addSprite("IconMetronomeAttacked", "metronome_attack", "artifacts", artReg);
            addSprite("IconMetronomeMoved", "metronome_move", "artifacts", artReg);
            addSprite("IconRemoteStarterUsed", "remoteStarter_used", "artifacts", artReg);


        }

        public void LoadManifest(IDeckRegistry registry) {
            // ExternalSprite.GetRaw((int)Spr.cards_colorless),
            ExternalSprite borderSprite = Sprites["NolaDeckFrame"] ?? throw new Exception();
            NolaDeck = new ExternalDeck(
                "Mezz.TwosCompany.NolaDeck",
                NolaColor,
                System.Drawing.Color.Black,
                Sprites["NolaDefaultCardSprite"] ?? ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(NolaDeck);

            borderSprite = Sprites["IsabelleDeckFrame"] ?? throw new Exception();
            IsabelleDeck = new ExternalDeck(
                "Mezz.TwosCompany.IsabelleDeck",
                IsabelleColor,
                System.Drawing.Color.Black,
                Sprites["IsabelleDefaultCardSprite"] ?? ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(IsabelleDeck);

            borderSprite = Sprites["IlyaDeckFrame"] ?? throw new Exception();
            IlyaDeck = new ExternalDeck(
                "Mezz.TwosCompany.IlyaDeck",
                IlyaColor,
                System.Drawing.Color.Black,
                Sprites["IlyaDefaultCardSprite"] ?? ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(IlyaDeck);

            borderSprite = Sprites["JostDeckFrame"] ?? throw new Exception();
            JostDeck = new ExternalDeck(
                "Mezz.TwosCompany.JostDeck",
                JostColor,
                System.Drawing.Color.Black,
                Sprites["JostDefaultCardSpriteUnsided"] ?? ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(JostDeck);

            borderSprite = Sprites["GaussDeckFrame"] ?? throw new Exception();
            GaussDeck = new ExternalDeck(
                "Mezz.TwosCompany.GaussDeck",
                GaussColor,
                System.Drawing.Color.Black,
                Sprites["GaussDefaultCardSprite"] ?? ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(GaussDeck);

            if (MoreDifficultiesApi != null) {
                MoreDifficultiesApi.RegisterAltStarters((Deck)NolaDeck.Id!, new StarterDeck() {
                    cards = new List<Card> { new Adaptation(), new CallAndResponse() }
                });
                MoreDifficultiesApi.RegisterAltStarters((Deck)IsabelleDeck.Id!, new StarterDeck() {
                    cards = new List<Card> { new Bind(), new MeasureBreak() }
                });
                MoreDifficultiesApi.RegisterAltStarters((Deck)IlyaDeck.Id!, new StarterDeck() {
                    cards = new List<Card> { new Pressure(), new ThermalBlast() }
                });
                MoreDifficultiesApi.RegisterAltStarters((Deck)JostDeck.Id!, new StarterDeck() {
                    cards = new List<Card> { new HighGuard(), new SweepingStrikes() },
                    artifacts = new List<Artifact> { new CrumpledWrit() }
                });
                MoreDifficultiesApi.RegisterAltStarters((Deck)GaussDeck.Id!, new StarterDeck() {
                    cards = new List<Card> { new KineticConduit(), new StrikeTwice() }
                });
            }

            Vault.charsWithLore.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("nola")), typeof(Deck)));
            Vault.charsWithLore.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("isabelle")), typeof(Deck)));
            Vault.charsWithLore.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("ilya")), typeof(Deck)));
            Vault.charsWithLore.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("gauss")), typeof(Deck)));

            BGRunWin.charFullBodySprites.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("nola")), typeof(Deck)),
                (Spr)(Manifest.Sprites["NolaFullbodySprite"].Id ?? throw new Exception("missing fullbody"))

            );
            BGRunWin.charFullBodySprites.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("isabelle")), typeof(Deck)),
                (Spr)(Manifest.Sprites["IsabelleFullbodySprite"].Id ?? throw new Exception("missing fullbody"))
            );

            BGRunWin.charFullBodySprites.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("ilya")), typeof(Deck)),
                (Spr)(Manifest.Sprites["IlyaFullbodySprite"].Id ?? throw new Exception("missing fullbody"))

            );
            BGRunWin.charFullBodySprites.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("gauss")), typeof(Deck)),
                (Spr)(Manifest.Sprites["GaussFullbodySprite"].Id ?? throw new Exception("missing fullbody"))
            );

            // MethodInfo get_unlocked_characters_postfix = typeof(CharacterRegistry).GetMethod("GetUnlockedCharactersPostfix", BindingFlags.Static | BindingFlags.NonPublic);
            /*
            Harmony harmony = new Harmony("Mezz.TwosCompany.Harmony.Charpatch");

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(StoryVars), nameof(StoryVars.GetUnlockedChars)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RelockChars))
            );*/
        }

        void ICardManifest.LoadManifest(ICardRegistry registry) {
            ManifHelper.DefineCards(0, 22, "Nola", NolaDeck ?? throw new Exception("missing deck"), Cards ?? throw new Exception("missing dictionary: cards"), Sprites, registry);
            ManifHelper.DefineCards(22, 27, "Isabelle", IsabelleDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
            ManifHelper.DefineCards(49, 23, "Ilya", IlyaDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
            ManifHelper.DefineCards(72, 24, "Jost", JostDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
            ManifHelper.DefineCards(96, 22, "Gauss", GaussDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
        }

        void IAnimationManifest.LoadManifest(IAnimationRegistry animReg) {
            if (NolaDeck == null || IsabelleDeck == null || IlyaDeck == null || JostDeck == null || GaussDeck == null)
                throw new Exception("missing deck");

            foreach (String emote in nolaEmotes)
                addEmoteAnim("nola", emote, animReg, NolaDeck);

            foreach (String emote in isabelleEmotes)
                addEmoteAnim("isabelle", emote, animReg, IsabelleDeck);

            foreach (String emote in ilyaEmotes)
                addEmoteAnim("ilya", emote, animReg, IlyaDeck);

            foreach (String emote in jostEmotes)
                addEmoteAnim("jost", emote, animReg, JostDeck);

            foreach (String emote in gaussEmotes)
                addEmoteAnim("gauss", emote, animReg, GaussDeck);

        }

        void ICharacterManifest.LoadManifest(ICharacterRegistry registry) {
            NolaCharacter = new ExternalCharacter("Mezz.TwosCompany.Character.Nola",
                NolaDeck ?? throw new Exception("Missing Deck"),
                Sprites["NolaFrame"] ?? throw new Exception("Missing Portrait"),
                // new Type[] { typeof(ReelIn), typeof(ClusterRocket) },
                new Type[] { typeof(Onslaught), typeof(Relentless) },
                new Type[0],
                Animations["NolaNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["NolaMiniAnim"] ?? throw new Exception("missing mini animation"));

            NolaCharacter.AddNameLocalisation("Nola");
            NolaCharacter.AddDescLocalisation(
                NolaColH + "NOLA</c>\nA tactician. Her cards are reliant on her crew,"
                + " capitalizing on their capabilities through <c=keyword>card and energy manipulation</c>."
            );

            registry.RegisterCharacter(NolaCharacter);

            IsabelleCharacter = new ExternalCharacter("Mezz.TwosCompany.Character.Isabelle",
                IsabelleDeck ?? throw new Exception("Missing Deck"),
                Sprites["IsabelleFrame"] ?? throw new Exception("Missing Portrait"),
                new Type[] { typeof(Sideswipe), typeof(Flourish) },
                new Type[0],
                Animations["IsabelleNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["IsabelleMiniAnim"] ?? throw new Exception("missing mini animation"));

            IsabelleCharacter.AddNameLocalisation("Isabelle");
            IsabelleCharacter.AddDescLocalisation(
                IsaColH + "ISABELLE</c>\nA rival mercenary. " +
                "Her cards often combine <c=keyword>attacks</c> with <c=keyword>movement</c>"
                + ", but rarely do only one or the other."
            );

            registry.RegisterCharacter(IsabelleCharacter);

            IlyaCharacter = new ExternalCharacter("Mezz.TwosCompany.Character.Ilya",
                IlyaDeck ?? throw new Exception("Missing Deck"),
                Sprites["IlyaFrame"] ?? throw new Exception("Missing Portrait"),
                new Type[] { typeof(MoltenShot), typeof(Galvanize) },
                new Type[0],
                Animations["IlyaNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["IlyaMiniAnim"] ?? throw new Exception("missing mini animation"));

            IlyaCharacter.AddNameLocalisation("Ilya");
            IlyaCharacter.AddDescLocalisation(
                IlyaColH + "ILYA</c>\nA former pirate. " +
                "His cards deal <c=keyword>heavy damage</c>, but require accruing and spending <c=downside>heat</c>."
            );

            registry.RegisterCharacter(IlyaCharacter);

            JostCharacter = new ExternalCharacter("Mezz.TwosCompany.Character.Jost",
                JostDeck ?? throw new Exception("Missing Deck"),
                Sprites["JostFrame"] ?? throw new Exception("Missing Portrait"),
                new Type[] { typeof(OverheadBlow), typeof(FrontGuard) },
                new Type[] { typeof(CrumpledWrit) },
                Animations["JostNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["JostMiniAnim"] ?? throw new Exception("missing mini animation"));

            JostCharacter.AddNameLocalisation("Jost");
            JostCharacter.AddDescLocalisation(
                JostColH + "JOST</c>\nA wandering spacer. " +
                "His cards <c=keyword>alternate</c> between <c=keyword>offense</c> and <c=keyword>defense</c>, requiring measured, rhythmic use."
            );

            registry.RegisterCharacter(JostCharacter);

            GaussCharacter = new ExternalCharacter("Mezz.TwosCompany.Character.Gauss",
                GaussDeck ?? throw new Exception("Missing Deck"),
                Sprites["GaussFrame"] ?? throw new Exception("Missing Portrait"),
                new Type[] { typeof(SparkCard), typeof(ConduitCard) },
                new Type[0],
                Animations["GaussNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["GaussMiniAnim"] ?? throw new Exception("missing mini animation"));

            GaussCharacter.AddNameLocalisation("Gauss");
            GaussCharacter.AddDescLocalisation(
                GaussColH + "GAUSS</c>\nA freelancing physicist. " +
                "Her cards utilize " + ChainColH + "chain lightning</c>, <c=keyword>arcing</c> between midrow objects with <c=keyword>versatile offense</c>."
            );

            registry.RegisterCharacter(GaussCharacter);
        }
    }
}
