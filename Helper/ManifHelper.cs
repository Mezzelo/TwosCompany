﻿using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.Helper {

    public static class ManifHelper {
        public static String[] cardNames = new string[] {
            // 0-20 Nola
            "Adaptation",
            "Anticipation",
            "AllHands",
            "BattlePlan",
            "CallAndResponse",
            "CaptainsOrders",
            "Contingency",
            "DoubleDown",
            "Foresight",
            "Encore",
            // "FlexibleDodge",
            "HoldOn",
            "LetLoose",
            "Onslaught",
            "OpeningGambit",
            "Outmaneuver",
            // "Practiced",
            // "Protocol",
            "Recalibrate",
            "Relentless",
            "SteadyOn",
            "SuddenShift",
            "UncannyEvasion",
            "WeakPoint",
            // 21-44: Isabelle
            "Bind",
            "CompoundAttack",
            // "Couch",
            "CoupDeGrace",
            "Dominance",
            "Expose",
            // "FalseOpening",
            "Fleche",
            "Flourish",
            "Grapple",
            "Harry",
            "Haymaker",
            "HookAndDrag",
            "Jab",
            "LightningStrikes",
            "MeasureBreak",
            "Misdirection",
            "PointDefense",
            "Rake",
            // "Reap",
            "Remise",
            "Riposte",
            "Shove",
            "Sideswipe",
            "WildStrikes",
            "WildStrike",
            "WildDodge",
            // 45 - 65: Ilya
            "Apex",
            "Backdraft",
            "BlastShield",
            "Burnout",
            "Cauterize",
            "DragonsBreath",
            "Fireball",
            "FlashDraw",
            "Galvanize",
            "Haze",
            "HeatTreatment",
            "Ignition",
            "Imbue",
            "Maul",
            "MoltenShot",
            "Pressure",
            "ReactorBurn",
            "Scars",
            "ThermalBlast",
            "Thermal Runaway",
            "Wildfire",
        };

        public static String[] cardLocs = new string[] {
            // 0-20: Nola
            "Adaptation",
            "Anticipation",
            "All Hands",
            "Battle Plan",
            "Call and Response",
            "Captain's Orders",
            "Contingency",
            "Double Down",
            "Foresight",
            "Encore",
            // "Flexible Dodge",
            "Hold On!",
            "Let Loose",
            "Onslaught",
            "Opening Gambit",
            "Outmaneuver",
            // "Practiced",
            // "Protocol",
            "Recalibrate",
            "Relentless",
            "Steady On",
            "Sudden Shift",
            "Uncanny Evasion",
            "Weak Point",
            // 21-44: Isabelle
            "Bind",
            "Compound Attack",
            // "Couch",
            "Coup de Grace",
            "Dominance",
            "Expose",
            // "False Opening",
            "Fleche",
            "Flourish",
            "Grapple",
            "Harry",
            "Haymaker",
            "Hook and Drag",
            "Jab",
            "Lightning Strikes",
            "Measure Break",
            "Misdirection",
            "Point Defense",
            "Rake",
            // "Reap",
            "Remise",
            "Riposte",
            "Shove",
            "Sideswipe",
            "Wild Strikes",
            "Wild Strike",
            "Wild Dodge",
            // 45 - 65: Ilya
            "Apex",
            "Backdraft",
            "Blast Shield",
            "Burnout",
            "Cauterize",
            "Dragon's Breath",
            "Fireball",
            "Flash Draw",
            "Galvanize",
            "Haze",
            "Heat Treatment",
            "Ignition",
            "Imbue",
            "Maul",
            "Molten Shot",
            "Pressure",
            "Reactor Burn",
            "Scars",
            "Thermal Blast",
            "Thermal Runaway",
            "Wildfire",
        };

        public static List<String> hasFlipSprite = new List<string> {
            "Adaptation",
            "Backdraft",
            "Flourish",
            "Haymaker",
            "HookAndDrag",
            "LightningStrikes",
            "MeasureBreak",
            "Sideswipe",
            "Shove",
            "SuddenShift",
        };

        public static Dictionary<string, string> cardTexts = new Dictionary<string, string> {
            {"Anticipation", "Temporarily reduce the cost of a card in your <c=keyword>draw pile</c> by <c=energy>3</c>."},
            {"AnticipationA", "Temporarily reduce the cost of a card in your <c=keyword>draw pile</c> by <c=energy>4</c>."},
            {"AnticipationB", "Temporarily reduce the cost of a card in your <c=keyword>draw pile</c> by <c=energy>3</c>."},
            {"AllHands", "Play your entire hand from <c=card>{0}</c> to <c=card>{1}</c> for free."},
            {"AllHandsA", "Play your entire hand from <c=card>{0}</c> to <c=card>{1}</c> for free."},
            {"AllHandsB", "Play your <c=card>{0}</c> card. Reduce this card's cost by <c=energy>3</c> once a turn."},
            {"BattlePlan", "Copy your hand on top of the <c=keyword>draw pile</c> with <c=cardtrait>temp</c> and <c=cardtrait>exhaust</c>."},
            {"BattlePlanA", "Copy your hand on top of the <c=keyword>draw pile</c> with <c=cardtrait>temp</c> and <c=cardtrait>exhaust</c>."},
            {"BattlePlanB", "Copy your hand on top of the <c=keyword>draw pile</c> with <c=cardtrait>temp</c> and <c=cardtrait>exhaust</c>."},
            {"CaptainsOrders", "Choose a card in your hand. Play it and <c=keyword>gain</c> its <c=keyword>cost</c>."},
            {"CaptainsOrdersA", "Choose a card in your hand. Play it and <c=keyword>gain</c> its <c=keyword>cost</c>. Draw <c=keyword>1</c> card."},
            {"CaptainsOrdersB", "Choose a card in your hand. Play it and <c=keyword>gain</c> its <c=keyword>cost</c>. Ignore <c=cardtrait>exhaust</c>."},
            {"CompoundAttack", "Add 4 <c=card>Jabs</c> to your hand. One is a <c=card>Fleche</c> in disguise."},
            {"CompoundAttackA", "Add 3 <c=card>Jabs</c> to your hand. One is a <c=card>Fleche</c> in disguise."},
            {"CompoundAttackB", "Add 5 <c=card>Jab B</c>s to your hand. One is a <c=card>Fleche B</c> in disguise."},
            {"DoubleDown", "Increase your current status effects by <c=keyword>1</c>."},
            {"DoubleDownA", "Increase your current status effects by <c=keyword>1</c>."},
            {"DoubleDownB", "<c=keyword>Double</c> your current status effects."},
            {"Foresight", "Choose <c=keyword>4</c> cards in your <c=keyword>draw pile</c> to <c=keyword>discard</c>. Draw <c=keyword>1</c> card."},
            {"ForesightA", "Choose <c=keyword>4</c> cards in your <c=keyword>draw pile</c> to <c=keyword>discard</c>. Draw <c=keyword>3</c>."},
            {"ForesightB", "<c=keyword>Exhaust 1</c> and <c=keyword>discard 3</c> cards in your <c=keyword>draw pile</c>. Draw <c=keyword>1</c> card."},
            {"LetLoose", "Temporarily reduce the cost of your entire hand by <c=energy>1</c>."},
            {"LetLooseA", "Draw <c=keyword>2</c> cards. Reduce the cost of your entire hand by <c=energy>1</c>."},
            {"LetLooseB", "Temporarily reduce the cost of your entire hand by <c=energy>2</c>."},
            {"Recalibrate", "Shuffle your <c=keyword>discard pile</c> back into your <c=keyword>draw pile</c>. Draw <c=keyword>2</c> cards."},
            {"RecalibrateA", "Shuffle your <c=keyword>discard pile</c> back into your <c=keyword>draw pile</c>. Draw <c=keyword>2</c> cards."},
            {"RecalibrateB", "Shuffle your <c=keyword>discard pile</c> back into your <c=keyword>draw pile</c>. Draw <c=keyword>4</c> cards."},
            {"Remise", "<c=cardtrait>X</c> = <c=cardtrait># of enemy attacks</c>. Gain {0} <c=status>evade</c>, fire for <c=hurt>{1}</c> dmg."},
            {"RemiseA", "<c=cardtrait>X</c> = <c=cardtrait># of enemy attacks</c>. Gain {0} <c=status>evade</c>, fire for <c=hurt>{1}</c> dmg."},
            {"RemiseB", "<c=cardtrait>X</c> = <c=cardtrait># of enemy attacks</c>. Gain {0} <c=status>evade</c>, deal <c=hurt>1</c> dmg <c=keyword>{1}</c> times."},
            {"FlexibleDodge", "<c=keyword>Flip</c> to change <c=keyword>autododge</c> direction. <c=keyword>Play</c> to avoid gaining <c=keyword>autododge</c>."},
            {"FlexibleDodgeA", "<c=keyword>Flip</c> to change <c=keyword>autododge</c> direction. <c=keyword>Play</c> to avoid gaining <c=keyword>autododge</c>."},
            {"FlexibleDodgeB", "<c=keyword>Flip</c> to change <c=keyword>autododge</c> direction. <c=keyword>Play</c> to avoid gaining <c=keyword>autododge</c>."},
            {"Wildfire", "Play your hand from <c=card>{0}</c> to <c=card>{1}</c>. Gain <c=downside>heat<c/> for each card."},
            {"WildfireA", "Play your hand from <c=card>{0}</c> to <c=card>{1}</c>. Gain <c=downside>heat<c/> for each, then lose 2."},
            {"WildfireB", "Play your  hand from <c=card>{0}</c> to <c=card>{1}</c>. Gain <c=downside>heat<c/> for each card."},
        };

        public static int numCards() {
            return cardNames.Length;
        }

        public static void DefineCardSprites(DirectoryInfo ModRootFolder, Dictionary<string, ExternalSprite> sprites) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            foreach (String cardName in cardNames) {
                sprites.Add((cardName + "CardSprite"),
                    new ExternalSprite("Mezz.TwosCompany.Sprites." + cardName + "CardSprite", new FileInfo(
                        Path.Combine(ModRootFolder.FullName, "Sprites", "cards", Path.GetFileName("mezz_" + cardName + ".png")))
                    )
                );
                if (hasFlipSprite.Contains(cardName)) {
                    sprites.Add((cardName + "CardSpriteFlip"),
                        new ExternalSprite("Mezz.TwosCompany.Sprites." + cardName + "CardSpriteFlip", new FileInfo(
                            Path.Combine(ModRootFolder.FullName, "Sprites", "cards", Path.GetFileName("mezz_" + cardName + "_flip.png")))
                        )
                    );
                }
            }
        }

        public static void DefineCards(int deckStart, int deckLength, string charName, 
            ExternalDeck deck, Dictionary<string, ExternalCard> Cards, Dictionary<string, ExternalSprite> Sprites,
            ICardRegistry registry) {
            for (int i = deckStart; i < deckStart + deckLength; i++) {
                if (Type.GetType("TwosCompany.Cards." + charName + "." + cardNames[i]) == null)
                    continue;
                Cards.Add(cardNames[i],
                    new ExternalCard("Mezz.TwosCompany.Cards." + charName + cardNames[i], 
                    Type.GetType("TwosCompany.Cards." + charName + "." + cardNames[i]) ?? throw new Exception("card type not found?: cardNames[i]"),
                    Sprites[(cardNames[i] + "CardSprite")], deck)
                );
                // note: might have to refactor this
                if (cardTexts.ContainsKey(cardNames[i])) {
                    Cards[cardNames[i]].AddLocalisation(cardLocs[i], cardTexts[cardNames[i]], cardTexts[cardNames[i] + "A"], cardTexts[cardNames[i] + "B"]);
                }
                else
                    Cards[cardNames[i]].AddLocalisation(cardLocs[i]);
                registry.RegisterCard(Cards[cardNames[i]]);
            }
        }
    }
}
