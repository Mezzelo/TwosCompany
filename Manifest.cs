﻿using CobaltCoreModding.Definitions;
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
    public class Manifest : ISpriteManifest, ICardManifest, IDeckManifest, ICharacterManifest, IAnimationManifest, IGlossaryManifest, IStatusManifest, IModManifest {
        public DirectoryInfo? ModRootFolder { get; set; }
        public DirectoryInfo? GameRootFolder { get; set; }

        public string Name { get; init; } = "Mezz.TwosCompany";
        public IEnumerable<DependencyEntry> Dependencies => Array.Empty<DependencyEntry>();
        public ILogger? Logger { get; set; }

        public static Dictionary<string, ExternalSprite> Sprites = new Dictionary<string, ExternalSprite>();
        public static Dictionary<string, ExternalAnimation> Animations = new Dictionary<string, ExternalAnimation>();

        public static Dictionary<string, ExternalCard>? Cards = new Dictionary<string, ExternalCard>();

        public static Dictionary<string, ExternalGlossary> Glossary = new Dictionary<string, ExternalGlossary>();

        public static Dictionary<string, ExternalStatus> Statuses = new Dictionary<string, ExternalStatus>();

        public static ExternalCharacter? NolaCharacter { get; private set; }
        public static ExternalDeck? NolaDeck { get; private set; }
        private static System.Drawing.Color NolaColor = System.Drawing.Color.FromArgb(23, 175, 198); // 17AFC6
        private static String NolaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", NolaColor.R, NolaColor.G, NolaColor.B.ToString("X2"));
        public static string[] nolaEmotes = new String[] {
            "mini", "neutral", "gameover", "angry", "annoyed", "getreal", "happy", "smug", "squint", "vengeful"
        };

        public static ExternalCharacter? IsabelleCharacter { get; private set; }
        public static ExternalDeck? IsabelleDeck { get; private set; }
        private static System.Drawing.Color IsabelleColor = System.Drawing.Color.FromArgb(47, 72, 183); // 2F48B7
        private static String IsaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", IsabelleColor.R, IsabelleColor.G, IsabelleColor.B);
        public static string[] isabelleEmotes = new String[] {
            "mini", "neutral", "gameover", "angry", "forlorn", "getreal", "glare", "shocked", "snide"
        };

        public static ExternalCharacter? IlyaCharacter { get; private set; }
        public static ExternalDeck? IlyaDeck { get; private set; }
        private static System.Drawing.Color IlyaColor = System.Drawing.Color.FromArgb(188, 84, 116); // BC5474
        private static String IlyaColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", IlyaColor.R, IlyaColor.G, IlyaColor.B.ToString("X2"));
        public static string[] ilyaEmotes = new String[] {
            "mini", "neutral", "gameover", "bashful", "blush", "happy", "intense", "shocked", "side", "squint"
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
        private void addStatus(string name, string displayName, string desc, bool isGood, 
            System.Drawing.Color mainColor, System.Drawing.Color? borderColor, IStatusRegistry statReg, bool timeStop) {
            Statuses.Add(name, new ExternalStatus("Mezz.TwosCompany." + name, isGood, 
                mainColor, borderColor.HasValue ? borderColor : null, Manifest.Sprites["Icon" + name], timeStop));
            Statuses[name].AddLocalisation(displayName, desc);
            statReg.RegisterStatus(Statuses[name]);
        }

        private void addGlossary(string name, string displayName, string desc, IGlossaryRegisty glossReg) {
            Glossary.Add(name, new ExternalGlossary("Mezz.TwosCompany." + name, name, false, ExternalGlossary.GlossayType.action, Manifest.Sprites["Icon" + name]));
            Glossary[name].AddLocalisation("en", displayName, desc);
            glossReg.RegisterGlossary(Glossary[name]);
        }

        public void BootMod(IModLoaderContact contact) {
            Harmony harmony = new Harmony("Mezz.TwosCompany.Harmony");

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.RenderAction)),
                prefix: new HarmonyMethod(typeof (PatchLogic), nameof(PatchLogic.Card_StatCostAction_Prefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AMove), nameof(AMove.Begin)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MoveBegin)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MoveEnd))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnBeginTurn)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.TurnBegin))
            );

            /*
            //create action draw code for agrow cluster
            var harmony = new Harmony("EWanderer.JohannaTheTrucker.AGrowClusterRendering");

            var card_render_action_method = typeof(Card).GetMethod("RenderAction", BindingFlags.Public | BindingFlags.Static) ?? throw new Exception();

            var card_render_action_prefix = this.GetType().GetMethod("AGrowClusterRenderActionPrefix", BindingFlags.NonPublic | BindingFlags.Static);

            harmony.Patch(card_render_action_method, prefix: new HarmonyMethod(card_render_action_prefix));
            */
        }

        void ISpriteManifest.LoadManifest(ISpriteRegistry artReg) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            // cardSprites
            ManifHelper.DefineCardSprites(ModRootFolder, Sprites);
            foreach (String cardName in ManifHelper.cardNames)
                artReg.RegisterArt(Sprites[(cardName + "CardSprite")]);
            foreach (String cardName in ManifHelper.hasFlipSprite)
                artReg.RegisterArt(Sprites[(cardName + "CardSpriteFlip")]);

            // manual def for toggle card art
            // addSprite("Adaptation_TopCardSprite", "Adaptation_Top", "cards", artReg);
            // addSprite("Adaptation_BottomCardSprite", "Adaptation_Bottom", "cards", artReg);

            // icons
            addSprite("IconEnergyPerCard", "energyPerCard", "icons", artReg);
            addSprite("IconEnergyPerPlay", "energyPerPlay", "icons", artReg);
            addSprite("IconRaiseCostHint", "energyPerPlay", "icons", artReg);
            addSprite("IconLowerPerPlay", "lowerPerPlay", "icons", artReg);
            addSprite("IconLowerCostHint", "lowerPerPlay", "icons", artReg);
            addSprite("IconTurnIncreaseCost", "turnIncreaseCost", "icons", artReg);
            addSprite("IconAllIncrease", "allIncrease", "icons", artReg);
            addSprite("IconAllIncreaseCombat", "allIncreaseCombat", "icons", artReg);
            addSprite("IconTempStrafe", "tempStrafe", "icons", artReg);
            addSprite("IconShieldCost", "shieldCost", "icons", artReg);
            addSprite("IconShieldCostOff", "shieldCostOff", "icons", artReg);
            addSprite("IconEvadeCost", "evadeCost", "icons", artReg);
            addSprite("IconEvadeCostOff", "evadeCostOff", "icons", artReg);
            addSprite("IconHeatCost", "heatCost", "icons", artReg);
            addSprite("IconHeatCostOff", "heatCostOff", "icons", artReg);
            addSprite("IconPointDefenseLeft", "pdLeft", "icons", artReg);
            addSprite("IconPointDefense", "pdRight", "icons", artReg);
            addSprite("IconCallAndResponseHint", "callAndResponseHint", "icons", artReg);

            addSprite("NolaFrame", "char_nola", "panels", artReg);
            addSprite("IsabelleFrame", "char_isabelle", "panels", artReg);
            addSprite("IlyaFrame", "char_ilya", "panels", artReg);

            addSprite("NolaDeckFrame", "border_nola", "cardshared", artReg);
            addSprite("IsabelleDeckFrame", "border_isabelle", "cardshared", artReg);
            addSprite("IlyaDeckFrame", "border_ilya", "cardshared", artReg);

            foreach (String emote in nolaEmotes)
                addCharSprite("nola", emote, "characters", artReg);

            foreach (String emote in isabelleEmotes)
                addCharSprite("isabelle", emote, "characters", artReg);

            foreach (String emote in ilyaEmotes)
                addCharSprite("ilya", emote, "characters", artReg);
            

        }

        public void LoadManifest(IDeckRegistry registry) {
            ExternalSprite borderSprite = Sprites["NolaDeckFrame"] ?? throw new Exception();
            NolaDeck = new ExternalDeck(
                "Mezz.TwosCompany.NolaDeck",
                NolaColor,
                System.Drawing.Color.Black,
                ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(NolaDeck);

            borderSprite = Sprites["IsabelleDeckFrame"] ?? throw new Exception();
            IsabelleDeck = new ExternalDeck(
                "Mezz.TwosCompany.IsabelleDeck",
                IsabelleColor,
                System.Drawing.Color.Black,
                ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(IsabelleDeck);

            borderSprite = Sprites["IlyaDeckFrame"] ?? throw new Exception();
            IlyaDeck = new ExternalDeck(
                "Mezz.TwosCompany.IlyaDeck",
                IlyaColor,
                System.Drawing.Color.Black,
                ExternalSprite.GetRaw((int)Spr.cards_colorless),
                borderSprite,
                null
            );
            registry.RegisterDeck(IlyaDeck);
        }

        void ICardManifest.LoadManifest(ICardRegistry registry) {
            ManifHelper.DefineCards(0, 21, "Nola", NolaDeck ?? throw new Exception("missing deck"), Cards ?? throw new Exception("missing dictionary: cards"), Sprites, registry);
            ManifHelper.DefineCards(21, 24, "Isabelle", IsabelleDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);
            ManifHelper.DefineCards(45, 21, "Ilya", IlyaDeck ?? throw new Exception("missing deck"), Cards, Sprites, registry);

            /*
            Cards.Add("Adaptation",
                new ExternalCard("Mezz.TwosCompany.Cards.Adaptation", 
                Type.GetType("TwosCompany.Cards.Nola.Adaptation") ?? throw new Exception("missing card type: adaptation"), Sprites["Adaptation_TopCardSprite"], NolaDeck)
            );
            Cards["Adaptation"].AddLocalisation("Adaptation");
            registry.RegisterCard(Cards["Adaptation"]);
            */


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
                new Type[] { typeof(LetLoose), typeof(SuddenShift) },
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
                new Type[] { typeof(ThermalBlast), typeof(Pressure) },
                new Type[0],
                Animations["IlyaNeutralAnim"] ?? throw new Exception("missing default animation"),
                Animations["IlyaMiniAnim"] ?? throw new Exception("missing mini animation"));

            IlyaCharacter.AddNameLocalisation(IlyaColH + "Ilya</c>");
            IlyaCharacter.AddDescLocalisation(
                IlyaColH + "ILYA</c>\nA former pirate. His cards are versatile and deal <c=keyword>heavy damage</c>, but accrue <c=downside>heat</c> and <c=downside>incur risk</c>."
            );

            registry.RegisterCharacter(IlyaCharacter);
        }
        public void LoadManifest(IStatusRegistry registry) {
            addStatus("TempStrafe", "Temporary Strafe", "Fire for {0} damage immediately after every move you make. <c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.Violet, System.Drawing.Color.FromArgb(unchecked((int)0xff5e5ce3)), registry, true);
            /* addStatus("Relentless", "Relentless", "Gain {0} <c=status>TEMPORARY SHIELD</c> for every card played this turn. <c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.Violet, null, registry, true);
            addStatus("Control", "Control", "Gain {0} <c=status>EVADE</c> each turn. If you don't hit your enemy before your turn ends, <c=downside>lose this status.</c>",
                true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("Encore", "Encore", "Play your next card <c=keyword>an additional time</c>. Reduce this status by <c=keyword>1</c> for every card played.",
                true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("EncoreB", "Encore B", "Play your next card <c=keyword>two more times</c>. Reduce this status by <c=keyword>1</c> for every card played.",
                true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("UncannyEvasion", "Uncanny Evasion", "Once per turn when shields are broken, gain {0} <c=status>AUTODODGE</c>.",
                true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("FalseOpening", "False Opening", "Gain {0} <c=status>OVERDRIVE</c> for every hit received this turn. <c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.Tomato, null, registry, true);
            addStatus("FalseOpeningB", "False Opening B", "Gain {0} <c=status>POWERDRIVE</c> for every hit received this turn. <c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.Wheat, null, registry, true);
            */
        }

        public void LoadManifest(IGlossaryRegisty registry) {
            addGlossary("EnergyPerCard", "Urgent",
                "This card's cost increases by <c=downside>{0}</c> this turn for every other card played while in your hand. Resets when played or discarded."
                , registry);
            addGlossary("EnergyPerPlay", "Rising Cost",
                "This card's cost increases by <c=downside>{0}</c> when played. Resets when discarded, or when combat ends."
                , registry);
            addGlossary("LowerCostHint", "Discount Other",
                "Lower a card's cost by <c=keyword>{0}</c> until played, or until combat ends."
                , registry);
            addGlossary("RaiseCostHint", "Expensive Other",
                "Raise a card's cost by <c=keyword>{0}</c> until played, or until combat ends."
                , registry);
            addGlossary("TurnIncreaseCost", "Timed Cost",
                "This card's cost increases by <c=keyword>{0}</c> every turn while held. Resets when played, discarded, or when combat ends."
                , registry);
            addGlossary("LowerPerPlay", "Lowering Cost",
                "This card's cost decreases by <c=keyword>{0}</c> when played. Resets when discarded, or when combat ends."
                , registry);
            addGlossary("AllIncrease", "Intensify",
                "All of this card's values increase by <c=keyword>{0}</c> when played. Resets when drawn, or when combat ends."
                , registry);
            addGlossary("AllIncreaseCombat", "Lasting Intensify",
                "All of this card's values increase by <c=keyword>{0}</c> when played. Resets <c=downside>when combat ends</c>."
                , registry);
            addGlossary("TempStrafe", "Temporary Strafe",
                "Your strafe decreases by <c=downside>{0}</c> at the start of your next turn."
                , registry);
            addGlossary("PointDefense", "Point Defense",
                "Align your cannon {0} to the {1} hostile <c=drone>midrow object</c> over your ship. " +
                "If there are none, <c=downside>discard instead</c>."
                , registry);
            addGlossary("CallAndResponseHint", "Call and Response",
                "Whenever you play this card, draw the selected card from the <c=keyword>draw or discard pile</c>{0}.\n" +
                "If the stored card was <c=cardtrait>exhausted</c> or <c=downside>single use</c>, choose another."
                , registry);
            addGlossary("ShieldCost", "Shield Cost",
                "Lose {0} <c=status>SHIELD</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("EvadeCost", "Evade Cost",
                "Lose {0} <c=status>EVADE</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("HeatCost", "Heat Cost",
                "Lose {0} <c=status>HEAT</c>. If you don't have enough, this action does not happen."
                , registry);
        }
    }
}
