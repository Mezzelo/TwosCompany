using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using TwosCompany.Helper;
using TwosCompany.Cards.Nola;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Ilya;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using System.Reflection;

namespace TwosCompany {
    public partial class Manifest : ISpriteManifest, ICardManifest, IDeckManifest, ICharacterManifest, IAnimationManifest,
        IGlossaryManifest, IStatusManifest, 
        ICustomEventManifest, IArtifactManifest, IModManifest {
        public DirectoryInfo? ModRootFolder { get; set; }
        public DirectoryInfo? GameRootFolder { get; set; }

        public string Name { get; init; } = "Mezz.TwosCompany";
        public IEnumerable<DependencyEntry> Dependencies => Array.Empty<DependencyEntry>();
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
            "mini", "neutral", "gameover", "crystallized", "nap", "angry", "annoyed", "getreal", "happy", "smug", "squint", "vengeful"
        };

        public static ExternalCharacter? IsabelleCharacter { get; private set; }
        public static ExternalDeck? IsabelleDeck { get; private set; }
        public static System.Drawing.Color IsabelleColor = System.Drawing.Color.FromArgb(47, 72, 183); // 2F48B7
        public static String IsaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", IsabelleColor.R, IsabelleColor.G, IsabelleColor.B);
        public static string[] isabelleEmotes = new String[] {
            "mini", "neutral", "gameover", "crystallized", "nap", "angry", "forlorn", "getreal", "glare", "happy", "swordhappy", "shocked", "snide", "squint", "swordsquint"
        };

        public static ExternalCharacter? IlyaCharacter { get; private set; }
        public static ExternalDeck? IlyaDeck { get; private set; }
        public static System.Drawing.Color IlyaColor = System.Drawing.Color.FromArgb(188, 84, 116); // BC5474
        public static String IlyaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", IlyaColor.R, IlyaColor.G, IlyaColor.B.ToString("X2"));
        public static string[] ilyaEmotes = new String[] {
            "mini", "neutral", "gameover", "crystallized", "nap", "bashful", "blush", "forlorn", "happy", "intense", "shocked", "side", "squint"
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
            foreach (String cardName in ManifHelper.cardNames) {
                if (!ManifHelper.defaultArtCards.Contains(cardName))
                    artReg.RegisterArt(Sprites[(cardName + "CardSprite")]);
            }
            foreach (String cardName in ManifHelper.hasFlipSprite)
                artReg.RegisterArt(Sprites[(cardName + "CardSpriteFlip")]);

            // manual def for toggle card art
            // addSprite("Adaptation_TopCardSprite", "Adaptation_Top", "cards", artReg);
            // addSprite("Adaptation_BottomCardSprite", "Adaptation_Bottom", "cards", artReg);

            // hint/cardaction icons
            addSprite("IconEnergyPerCard", "energyPerCard", "icons", artReg);
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
            addSprite("IconPointDefenseLeft", "pdLeft", "icons", artReg);
            addSprite("IconPointDefense", "pdRight", "icons", artReg);
            addSprite("IconCallAndResponseHint", "callAndResponseHint", "icons", artReg);
            addSprite("IconDisguisedHint", "disguisedHint", "icons", artReg);
            addSprite("IconDisguisedPermaHint", "disguisedPermaHint", "icons", artReg);

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
            // chars
            addSprite("NolaFrame", "char_nola", "panels", artReg);
            addSprite("IsabelleFrame", "char_isabelle", "panels", artReg);
            addSprite("IlyaFrame", "char_ilya", "panels", artReg);

            addSprite("NolaDeckFrame", "border_nola", "cardshared", artReg);
            addSprite("IsabelleDeckFrame", "border_isabelle", "cardshared", artReg);
            addSprite("IlyaDeckFrame", "border_ilya", "cardshared", artReg);

            addSprite("NolaDefaultCardSprite", "default_nola", "cards", artReg);
            addSprite("IsabelleDefaultCardSprite", "default_isabelle", "cards", artReg);
            addSprite("IlyaDefaultCardSprite", "default_ilya", "cards", artReg);

            addSprite("NolaFullbodySprite", "nola_end", "fullchars", artReg);
            addSprite("IsabelleFullbodySprite", "isabelle_end", "fullchars", artReg);

            foreach (String emote in nolaEmotes)
                addCharSprite("nola", emote, "characters", artReg);

            foreach (String emote in isabelleEmotes)
                addCharSprite("isabelle", emote, "characters", artReg);

            foreach (String emote in ilyaEmotes)
                addCharSprite("ilya", emote, "characters", artReg);

            // artifact icons
            foreach (String artifact in ManifArtifactHelper.artifactNames)
                addSprite("Icon" + artifact, string.Concat(artifact[0].ToString().ToLower(), artifact.AsSpan(1)), "artifacts", artReg);

            // variable artifact icons
            addSprite("IconMetronomeAttacked", "metronome_attack", "artifacts", artReg);
            addSprite("IconMetronomeMoved", "metronome_move", "artifacts", artReg);


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


            Vault.charsWithLore.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("nola")), typeof(Deck)));
            Vault.charsWithLore.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("isabelle")), typeof(Deck)));

            BGRunWin.charFullBodySprites.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("nola")), typeof(Deck)),
                (Spr)(Manifest.Sprites["NolaFullbodySprite"].Id ?? throw new Exception("missing fullbody"))

            );
            BGRunWin.charFullBodySprites.Add((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("isabelle")), typeof(Deck)),
                (Spr)(Manifest.Sprites["IsabelleFullbodySprite"].Id ?? throw new Exception("missing fullbody"))
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
            ManifHelper.DefineCards(22, 26, "Isabelle", IsabelleDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
            ManifHelper.DefineCards(48, 23, "Ilya", IlyaDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
        }

        void IAnimationManifest.LoadManifest(IAnimationRegistry animReg) {
            if (NolaDeck == null || IsabelleDeck == null || IlyaDeck == null)
                throw new Exception("missing deck");

            foreach (String emote in nolaEmotes)
                addEmoteAnim("nola", emote, animReg, NolaDeck);

            foreach (String emote in isabelleEmotes)
                addEmoteAnim("isabelle", emote, animReg, IsabelleDeck);

            foreach (String emote in ilyaEmotes)
                addEmoteAnim("ilya", emote, animReg, IlyaDeck);
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

            NolaCharacter.AddNameLocalisation(NolaColH + "Nola</c>");
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

            IsabelleCharacter.AddNameLocalisation(IsaColH + "Isabelle</c>");
            IsabelleCharacter.AddDescLocalisation(
                IsaColH + "ISABELLE</c>\nA rival mercenary. Her cards often combine <c=keyword>attacks</c> with <c=keyword>movement</c>"
                + ", but rarely only do one or the other."
            );

            registry.RegisterCharacter(IsabelleCharacter);

            IlyaCharacter = new ExternalCharacter("Mezz.TwosCompany.Character.Ilya",
                IlyaDeck ?? throw new Exception("Missing Deck"),
                Sprites["IlyaFrame"] ?? throw new Exception("Missing Portrait"),
                new Type[] { typeof(MoltenShot), typeof(Galvanize) },
                new Type[0],
                Animations["IlyaNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["IlyaMiniAnim"] ?? throw new Exception("missing mini animation"));

            IlyaCharacter.AddNameLocalisation(IlyaColH + "Ilya</c>");
            IlyaCharacter.AddDescLocalisation(
                IlyaColH + "ILYA</c>\nA former pirate. His cards are versatile and deal <c=keyword>heavy damage</c>, but accrue <c=downside>heat</c> and <c=downside>incur risk</c>."
            );

            registry.RegisterCharacter(IlyaCharacter);
        }
    }
}
