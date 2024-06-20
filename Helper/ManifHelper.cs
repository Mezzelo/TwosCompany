﻿using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwosCompany.Cards;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Helper {

    public static class ManifHelper {

        public static int[] deckSize = new int[] {
            24,
            31,
            25,
            28,
            23
        };
        public static int getDeckSum(int index) => deckSize[index] + (index > 0 ? getDeckSum(index - 1) : 0);

        public static Dictionary<string, string> cardNames = new Dictionary<string, string> {
            // 0-23 Nola
            { "Adaptation", "Adaptation"},
            { "Anticipation", "Anticipation"},
            { "AllHands", "All Hands"},
            { "BattlePlan", "Battle Plan"},
            { "CallAndResponse", "Call and Response"},
            { "CaptainsOrders", "Captain's Orders"},
            { "Contingency", "Contingency"},
            { "DamageControl", "Damage Control"},
            { "DoubleDown", "Double Down"},
            { "FollowThrough", "Follow Through"},
            { "Foresight", "Foresight"},
            { "Guidance", "Guidance"},
            { "HoldOn", "Hold On!"},
            { "LetLoose", "Let Loose"},
            { "OnYourMark", "Take The Lead"},
            { "Onslaught", "Onslaught"},
            { "OpeningGambit", "Opening Gambit"},
            { "Outmaneuver", "Outmaneuver"},
            { "Recalibrate", "Recalibrate"},
            { "Relentless", "Relentless"},
            { "Ruminate", "Ruminate"},
            { "SteadyOn", "Steady On"},
            { "SuddenShift", "Sudden Shift"},
            { "WeakPoint", "Weak Point"},
            // 22-49: Isabelle
            { "BladeDance", "Blade Dance"},
            { "Bind", "Bind"},
            { "Cascade", "Cascade"},
            { "CompoundAttack", "Compound Attack"},
            { "Couch", "Couch"},
            { "CoupDeGrace", "Coup de Grace"},
            { "EnGarde", "En Garde"},
            { "Expose", "Expose"},
            { "FalseOpening", "False Opening"},
            { "Finesse", "Finesse"},
            { "Fleche", "Fleche"},
            { "Flourish", "Flourish"},
            { "Grapple", "Grapple"},
            { "Harry", "Harry"},
            { "Haymaker", "Haymaker"},
            { "HookAndDrag", "Hook and Drag"},
            { "Jab", "Jab"},
            { "LightningStrikes", "Lightning Strikes"},
            { "MeasureBreak", "Measure Break"},
            { "Misdirection", "Misdirection"},
            { "PointDefense", "Point Defense"},
            { "Rake", "Rake"},
            { "Recover", "Recover"},
            { "Remise", "Remise"},
            { "Riposte", "Riposte"},
            { "Shove", "Shove"},
            { "Sideswipe", "Sideswipe"},
            { "Taunt", "Goad"},
            { "WildStrikes", "Wild Strikes"},
            { "WildStrike", "Wild Strike"},
            { "WildDodge", "Wild Dodge"},
            // 49 - 71: Ilya
            { "Apex", "Apex"},
            { "Backdraft", "Backdraft"},
            { "BlastShield", "Blast Shield"},
            { "Burnout", "Burnout"},
            { "Cauterize", "Cauterize"},
            { "DragonsBreath", "Dragon's Breath"},
            { "Ember", "Ember"},
            { "HeatRecycler", "Heat Recycler"},
            { "Fireball", "Fireball"},
            { "FlashDraw", "Flash Draw"},
            { "Galvanize", "Galvanize"},
            { "Haze", "Haze"},
            { "HeatTreatment", "Heat Treatment"},
            { "Ignition", "Ignition"},
            { "Imbue", "Imbue"},
            { "Immolate", "Immolate"},
            { "MagmaInjection", "Searing Rupture"},
            { "Maul", "Maul"},
            { "MoltenShot", "Molten Shot"},
            { "Pressure", "Pressure"},
            { "ReactorBurn", "Reactor Burn"},
            { "Scars", "Scars"},
            { "ThermalBlast", "Thermal Blast"},
            { "ThermalRunaway", "Thermal Runaway"},
            { "Wildfire", "Wildfire"},
            // 72 - 95: Jost
            { "Backstep", "Backstep"},
            { "BattleTempo", "Battle Tempo"},
            { "BreatheIn", "Breathe In"},
            { "Challenge", "Challenge"},
            { "Commit", "Commit"},
            { "FollowMyLead", "Follow My Lead"},
            { "Footwork", "Footwork"},
            { "Fortress", "Fortress"},
            { "FreedomOfMovement", "Freedom of Movement"},
            { "FrontGuard", "Front Guard"},
            { "HackAndSlash", "Hack and Slash"},
            { "Heartbeat", "Heartbeat"},
            { "HighGuard", "High Guard"},
            { "KeepFighting", "Keep Fighting"},
            { "Limitless", "Limitless"},
            { "MoveAsOne", "Move as One"},
            { "OffBalance", "Off Balance"},
            { "OverheadBlow", "Overhead Blow"},
            { "PommelBlow", "Pommel Blow"},
            { "Practiced", "Practiced"},
            { "ReactiveDefense", "Reactive Defense"},
            { "RecklessAbandon", "Reckless Abandon"},
            { "RegainPoise", "Regain Poise"},
            { "Retribution", "Retribution"},
            { "RisingFlame", "Rising Storm"},
            { "RushOfBattle", "Rush of Battle"},
            { "StandFirm", "Stand Firm"},
            { "SweepingStrikes", "Sweeping Strikes"}, 
            // 96 - 117: Gauss
            { "AsteroidBelt", "Asteroid Belt" },
            { "Autocurrent", "Autocurrent" },
            { "BlindGrab", "Whatever Works" },
            { "ConductorArray", "Conduit Array" },
            { "ConductorField", "Conductor Field" },
            { "ConduitCard", "Conduit" },
            { "DistantStrike", "Distant Strike" },
            { "Electrocute", "Electrocute" },
            { "Feedback", "Feedback Conduit" },
            { "Gravitate", "Gravitate" },
            { "KineticConduit", "Kinetic Conduit" },
            { "HeavyBolt", "Heavy Bolt" },
            { "HyperspaceStorm", "Hyperspace Storm" },
            { "HyperspaceWind", "Hyperspace Wind" },
            { "LatentEnergy", "Latent Energy" },
            { "ReversePolarity", "Reverse Polarity" },
            { "ShieldConduit", "Shield Conduit" },
            { "SolderShuffle", "Solder Shuffle" },
            { "SparkCard", "Spark" },
            { "StaticCharge", "Static Charge" },
            { "StrikeTwice", "Strike Twice" },
            { "Tempest", "Tempest" },
            { "TremblingAirs", "Trembling Airs" },

        };

        public static string[] hasFlipSprite = new string[] {
            "Adaptation",
            "Backdraft",
            "ConductorArray",
            "DragonsBreath",
            "EnGarde",
            "HeatRecycler",
            "Flourish",
            "Grapple",
            "Gravitate",
            "Haymaker",
            "HookAndDrag",
            "HyperspaceWind",
            "LightningStrikes",
            "PointDefense",
            "Recover",
            "ShieldConduit",
            "Shove",
            "Sideswipe",
            "SolderShuffle",
            "StandFirm",
            "SuddenShift",
        };

        public static Dictionary<string, string> jostQuads = new Dictionary<string, string> {
            { "FollowMyLead", "Down1" },
            { "Heartbeat", "Down1" },
            { "HighGuard", "Down1" },
            { "ReactiveDefense", "Up1" },
            { "RisingFlame", "Up1" },
            { "HackAndSlash", "Up1" },
            { "Retribution", "Down1" },
        };

        public static string[] defaultArtCards = new string[] {
            "Anticipation",
            "AllHands",
            "AsteroidBelt",
            "BattlePlan",
            "BladeDance",
            "BlindGrab",
            "CaptainsOrders",
            "Cascade",
            "Couch",
            "DoubleDown",
            "Foresight",
            "Guidance",
            "LatentEnergy",
            "LetLoose",
            "Outmaneuver",
            "Recalibrate",
            "Remise",
            "Ruminate",
            "Tempest",
            "TremblingAirs",
            "Wildfire",

            "Backstep",
            "BattleTempo",
            "Challenge",
            "Commit",
            "FollowMyLead",
            "Fortress",
            "FreedomOfMovement",
            "FrontGuard",
            "HackAndSlash",
            "Heartbeat",
            "HighGuard",
            "KeepFighting",
            "Limitless",
            "MoveAsOne",
            "OnFumes",
            "OverheadBlow",
            "PommelBlow",
            "ReactiveStrike",
            "RecklessAbandon",
            "Retribution",
            "RushOfBattle",
            "StrengtheningStrikes",
            "SweepingStrikes",
            "Windup",
        };

        public static string[] defaultJost = new string[] {
            "BattleTempo",
            "FreedomOfMovement",
            "Limitless",
            "RushOfBattle",
            "StandFirm",
        };

        public static Dictionary<string, string> cardTexts = new Dictionary<string, string> {
            {"Anticipation", "Temporarily reduce the cost of a card in your <c=keyword>draw pile</c> by <c=energy>2</c>."},
            {"AnticipationA", "Temporarily reduce the cost of a card in your <c=keyword>draw pile</c> by <c=energy>1</c>, twice."},
            {"AnticipationB", "Temporarily reduce the cost of a card in your <c=keyword>draw pile</c> by <c=energy>2</c>."},
            {"AllHands", "Play your current hand from <c=card>{0}</c> to <c=card>{1}</c> for free."},
            {"AllHandsA", "Play your current hand from <c=card>{0}</c> to <c=card>{1}</c> for free."},
            {"AllHandsB", "Play your current hand from <c=card>{0}</c> to <c=card>{1}</c> for free."},
            {"AsteroidBelt", "Surround all <c=midrow>midrow objects</c> over your ship with <c=midrow>asteroids.</c>"},
            {"AsteroidBeltA", "Surround all <c=midrow>midrow objects</c> over your ship with <c=midrow>asteroids.</c>"},
            {"AsteroidBeltB", "Surround all <c=midrow>midrow objects</c> over your ship with <c=keyword>bubbled</c> <c=midrow>asteroids.</c>"},
            {"BattlePlan", "Copy your hand on top of the <c=keyword>draw pile</c> with <c=cardtrait>temp</c> and <c=cardtrait>exhaust</c>."},
            {"BattlePlanA", "Copy your hand on top of the <c=keyword>draw pile</c> with <c=cardtrait>temp</c> and <c=cardtrait>exhaust</c>."},
            {"BattlePlanB", "Copy your hand on top of the <c=keyword>draw pile</c> with <c=cardtrait>temp</c> and <c=cardtrait>exhaust</c>."},
            {"BladeDance", "Put 2 <c=cardtrait>temp</c>, <c=cardtrait>discounted</c> <c=cardtrait>exhaustable</c> <c=card>Flourishes</c> in your hand."},
            {"BladeDanceA", "Put 2 <c=cardtrait>temp</c>, <c=cardtrait>discounted</c> <c=cardtrait>exhaustable</c> <c=card>Flourish As</c> in your hand."},
            {"BladeDanceB", "Put 3 <c=cardtrait>temp</c>, <c=cardtrait>discounted</c> <c=cardtrait>exhaustable</c> <c=card>Flourishes</c> in your hand."},
            {"BlindGrab", "Draw a random <c=midrow>midrow launch</c> card from draw or discard."},
            {"BlindGrabA", "Draw a random <c=midrow>midrow launch</c> card from draw or discard."},
            {"BlindGrabB", "Draw two random <c=midrow>midrow launch</c> cards from draw or discard."},
            {"BreatheIn", "Move all <c=card>Heartbeats</c> to hand. They retain until played."},
            {"BreatheInA", "Move all <c=card>Heartbeats</c> to hand. They retain until\nplayed."},
            {"BreatheInB", "Move all <c=card>Heartbeats</c> to hand. They retain until played."},
            {"CaptainsOrders", "Choose a card in your hand. Play it and <c=keyword>gain</c> its <c=keyword>base cost</c>."},
            {"CaptainsOrdersA", "Choose a card in your hand. Play it and <c=keyword>gain</c> its <c=keyword>base cost</c>. Draw <c=keyword>2</c>."},
            {"CaptainsOrdersB", "Choose a card in hand. Play it, <c=keyword>gain</c> its <c=keyword>base cost</c>. Ignore <c=cardtrait>exhaust</c>."},
            {"Cascade", "Attack for <c=hurt>{1}</c> damage and move <c=keyword>{0}</c>, until you miss or fail to move."},
            {"CascadeA", "Attack for <c=hurt>{1}</c> damage and move <c=keyword>{0}</c>, until you miss or fail to move."},
            {"CascadeB", "Attack for <c=hurt>{1}</c> damage and move <c=keyword>{0}</c>, until you miss or fail to move."},
            {"CompoundAttack", "Add 4 <c=card>Jabs</c> to your hand. One is a <c=card>Fleche</c> in disguise."},
            {"CompoundAttackA", "Add 3 <c=card>Jabs</c> to your hand. One is a <c=card>Fleche</c> in disguise."},
            {"CompoundAttackB", "Add 4 <c=card>Jab B</c>s to your hand. One is a <c=card>Fleche B</c>. Cards <c=downside>don't undisguise.</c>"},
            {"Couch", "Deal <c=hurt>damage</c> equal to total dist. moved since drawing\nthis card{0}."},
            {"CouchA", "Deal <c=hurt>damage</c> equal to <c=keyword>2x</c> total dist. moved since drawing\nthis card{0}."},
            {"CouchB", "Deal <c=hurt>damage</c> equal to total dist. moved since drawing\nthis card{0}."},
            {"DoubleDown", "Increase your current status effects by <c=keyword>1</c>."},
            {"DoubleDownA", "Increase your current status effects by <c=keyword>1</c>."},
            {"DoubleDownB", "<c=keyword>Double</c> your current status effects."},
            {"Foresight", "Choose <c=keyword>3</c> cards in your <c=keyword>draw pile</c> to <c=keyword>discard</c>. Draw <c=keyword>1</c> card."},
            {"ForesightA", "Choose <c=keyword>3</c> cards in your <c=keyword>draw pile</c> to <c=keyword>discard</c>. Draw <c=keyword>2</c>."},
            {"ForesightB", "<c=keyword>Exhaust 1</c> and <c=keyword>discard 2</c> cards in your <c=keyword>draw pile</c>. Draw <c=keyword>1</c> card."},
            {"Guidance", "<c=keyword>Upgrade</c> all unupgraded cards in hand to <c=card>A</c> this combat."},
            {"GuidanceA", "<c=keyword>Upgrade</c> all unupgraded cards in hand to <c=card>A</c> this combat."},
            {"GuidanceB", "<c=keyword>Upgrade</c> all unupgraded cards in hand to <c=card>B</c> this combat."},
            {"LatentEnergy", "Put a <c=cardtrait>discounted</c> <c=card>Spark</c> in your hand."},
            {"LatentEnergyA", "Put a <c=cardtrait>discounted</c> <c=card>Spark A</c> in your hand."},
            {"LatentEnergyB", "Put a <c=cardtrait>discounted</c> <c=card>Spark B</c> in your hand."},
            {"LetLoose", "Discount all cards in your hand by <c=energy>1</c> energy, to a minimum of <c=keyword>1</c>."},
            {"LetLooseA", "Draw <c=keyword>2</c>. Discount your hand by <c=energy>1</c> energy, to a minimum of <c=keyword>1</c>."},
            {"LetLooseB", "Discount all cards in your hand by <c=energy>2</c> energy."},
            {"Outmaneuver", "Gain <c=keyword>{0}</c> <c=status>evade</c> for each attack targeting your ship{1}."},
            {"OutmaneuverA", "Gain <c=keyword>{0}</c> <c=status>evade</c> for each attack targeting your ship{1}."},
            {"OutmaneuverB", "Gain <c=keyword>{0}</c> <c=status>evade</c> for each attack targeting your ship{1}."},
            {"Recalibrate", "Shuffle your <c=keyword>discard pile</c> back into your <c=keyword>draw pile</c>. Draw <c=keyword>2</c> cards."},
            {"RecalibrateA", "Shuffle your <c=keyword>discard pile</c> back into your <c=keyword>draw pile</c>. Draw <c=keyword>2</c> cards."},
            {"RecalibrateB", "Shuffle your <c=keyword>discard pile</c> back into your <c=keyword>draw pile</c>. Draw <c=keyword>4</c> cards."},
            {"Remise", "<c=cardtrait>X</c> = <c=cardtrait># of enemy attacks</c>. Gain {0} <c=status>evade</c>, fire for <c=hurt>{1}</c> dmg."},
            {"RemiseA", "<c=cardtrait>X</c> = <c=cardtrait># of enemy attacks</c>. Gain {0} <c=status>evade</c>, fire for <c=hurt>{1}</c> dmg."},
            {"RemiseB", "<c=cardtrait>X</c> = <c=cardtrait># of enemy attacks</c>. Gain {0} <c=status>evade</c>, deal <c=hurt>{1}</c> dmg <c=keyword>{2}</c> times."},
            {"Ruminate", "Move <c=keyword>2</c> cards in hand to the top of your draw pile."},
            {"RuminateA", "Move <c=keyword>2</c> cards to the top of your draw pile. Draw <c=keyword>2</c> more next turn."},
            {"RuminateB", "Move <c=keyword>2</c> cards to the top of your draw pile and discount them by <c=energy>1</c>."},
            {"RushOfBattle", "Put 3 <c=cardtrait>temp</c> <c=card>Heartbeats</c> in your hand."},
            {"RushOfBattleA", "Put 3 <c=cardtrait>temp</c> <c=card>Heartbeats</c> in your hand."},
            {"RushOfBattleB", "Put 3 <c=cardtrait>temp</c> <c=card>Heartbeats</c> in your hand. They retain until played."},
            {"FlexibleDodge", "<c=keyword>Flip</c> to change <c=keyword>autododge</c> direction. <c=keyword>Play</c> to avoid gaining <c=keyword>autododge</c>."},
            {"FlexibleDodgeA", "<c=keyword>Flip</c> to change <c=keyword>autododge</c> direction. <c=keyword>Play</c> to avoid gaining <c=keyword>autododge</c>."},
            {"FlexibleDodgeB", "<c=keyword>Flip</c> to change <c=keyword>autododge</c> direction. <c=keyword>Play</c> to avoid gaining <c=keyword>autododge</c>."},
            {"Tempest", "Put 3 <c=card>Sparks</c> in your draw pile."},
            {"TempestA", "Put 3 <c=card>Sparks</c> in your hand."},
            {"TempestB", "Put 3 <c=cardtrait>discounted</c> <c=card>Sparks</c> in your draw pile."},
            {"TremblingAirs", "Move all " + Manifest.ChainColH +"chain lightning</c> cards to hand."},
            {"TremblingAirsA", "Move all " + Manifest.ChainColH + "chain lightning</c> cards to hand."},
            {"TremblingAirsB", "Move all " + Manifest.ChainColH + "lightning</c> cards to hand and discount them by 1."},
            {"Wildfire", "Play your hand from <c=card>{0}</c> to <c=card>{1}</c>. Gain <c=downside>HEAT</c> for each card."},
            {"WildfireA", "Play your hand from <c=card>{0}</c> to <c=card>{1}</c>. Gain <c=downside>HEAT</c> for each, then lose 2."},
            {"WildfireB", "Play your hand from <c=card>{0}</c> to <c=card>{1}</c>. Gain <c=downside>HEAT</c> for each card."},
        };

        public static Dictionary<string, string> charStoryNames = new Dictionary<string, string> {
            { "nola", "Mezz.TwosCompany.NolaDeck" },
            { "isabelle", "Mezz.TwosCompany.IsabelleDeck" },
            { "isa", "Mezz.TwosCompany.IsabelleDeck" },
            { "ilya", "Mezz.TwosCompany.IlyaDeck" },
            { "jost", "Mezz.TwosCompany.JostDeck" },
            { "gauss", "Mezz.TwosCompany.GaussDeck" },
            { "johanna", "JohannaTheTrucker.JohannaDeck" },
            { "jo", "JohannaTheTrucker.JohannaDeck" }
        };
        public static int GetDeckId(string deck) {
            if (deck == "nola")
                return (int)Manifest.NolaDeck!.Id!;
            else if (deck == "isa" || deck == "isabelle")
                return (int)Manifest.IsabelleDeck!.Id!;
            else if (deck == "ilya")
                return (int)Manifest.IlyaDeck!.Id!;
            else if (deck == "jost")
                return (int)Manifest.JostDeck!.Id!;
            else if (deck == "gauss")
                return (int)Manifest.GaussDeck!.Id!;
            return 0;
        }

        public static int numCards() {
            return cardNames.Keys.Count;
        }

        public static void DefineCardSprites(DirectoryInfo ModRootFolder, Dictionary<string, ExternalSprite> sprites) {
            if (ModRootFolder == null)
                throw new Exception("Root Folder not set");

            foreach (String cardName in cardNames.Keys) {
                if (!ManifHelper.defaultArtCards.Contains(cardName)) {
                    sprites.Add((cardName + "CardSprite"),
                        new ExternalSprite("Mezz.TwosCompany.Sprites." + cardName + "CardSprite", new FileInfo(
                            Path.Combine(ModRootFolder.FullName, "Sprites", "cards", Path.GetFileName("mezz_" + cardName + ".png")))
                        )
                    );
                    if (hasFlipSprite.Contains(cardName) || (jostQuads.ContainsKey(cardName) && jostQuads[cardName].Equals("unique")))
                        sprites.Add((cardName + "CardSpriteFlip"),
                            new ExternalSprite("Mezz.TwosCompany.Sprites." + cardName + "CardSpriteFlip", new FileInfo(
                                Path.Combine(ModRootFolder.FullName, "Sprites", "cards", Path.GetFileName("mezz_" + cardName + "_flip.png")))
                            )
                        );

                    if (jostQuads.ContainsKey(cardName) && jostQuads[cardName].Equals("unique")) {
                        sprites.Add((cardName + "CardSpriteBoth"),
                            new ExternalSprite("Mezz.TwosCompany.Sprites." + cardName + "CardSpriteBoth", new FileInfo(
                                Path.Combine(ModRootFolder.FullName, "Sprites", "cards", Path.GetFileName("mezz_" + cardName + "_both.png")))
                            )
                        );
                        sprites.Add((cardName + "CardSpriteNeither"),
                            new ExternalSprite("Mezz.TwosCompany.Sprites." + cardName + "CardSpriteNeither", new FileInfo(
                                Path.Combine(ModRootFolder.FullName, "Sprites", "cards", Path.GetFileName("mezz_" + cardName + "_neither.png")))
                            )
                        );
                    }
                }
            }
        }

        public static void DefineCards(int deckStart, int deckLength, string charName, 
            ExternalDeck deck, Dictionary<string, ExternalCard> Cards, Dictionary<string, ExternalSprite> Sprites,
            ICardRegistry registry) {
            for (int i = deckStart; i < deckStart + deckLength && i < cardNames.Keys.Count; i++) {
                if (Type.GetType("TwosCompany.Cards." + charName + "." + cardNames.ElementAt(i).Key) == null)
                    continue;
                string cardArtKey = "default";
                if (!defaultArtCards.Contains(cardNames.ElementAt(i).Key)) {
                    if (jostQuads.ContainsKey(cardNames.ElementAt(i).Key) && !jostQuads[cardNames.ElementAt(i).Key].Equals("unique")) {
                        cardArtKey = "JostDefaultCardSprite" + jostQuads[cardNames.ElementAt(i).Key];
                    } else
                        cardArtKey = cardNames.ElementAt(i).Key + "CardSprite";
                }

                Cards.Add(cardNames.ElementAt(i).Key,
                    new ExternalCard("Mezz.TwosCompany.Cards." + charName + cardNames.ElementAt(i).Key, 
                    Type.GetType("TwosCompany.Cards." + charName + "." + cardNames.ElementAt(i).Key) ?? throw new Exception("card type not found?: cardNames[i]"),
                    defaultArtCards.Contains(cardNames.ElementAt(i).Key) ? 
                        (defaultJost.Contains(cardNames.ElementAt(i).Key) || !charName.Equals("Jost") ? deck.CardArtDefault : Sprites["JostDefaultCardSprite"]) : 
                        Sprites[cardArtKey], deck)
                );
                // note: might have to refactor this
                if (cardTexts.ContainsKey(cardNames.ElementAt(i).Key)) {
                    Cards[cardNames.ElementAt(i).Key].AddLocalisation(cardNames.ElementAt(i).Value, cardTexts[cardNames.ElementAt(i).Key], 
                        cardTexts[cardNames.ElementAt(i).Key + "A"], cardTexts[cardNames.ElementAt(i).Key + "B"]);
                }
                else
                    Cards[cardNames.ElementAt(i).Key].AddLocalisation(cardNames.ElementAt(i).Value);
                registry.RegisterCard(Cards[cardNames.ElementAt(i).Key]);
            }
        }
    }
}
