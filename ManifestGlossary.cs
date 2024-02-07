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
    public partial class Manifest : IGlossaryManifest, IStatusManifest {

        public static Dictionary<string, ExternalStatus> Statuses = new Dictionary<string, ExternalStatus>();
        public static Dictionary<string, ExternalGlossary> Glossary = new Dictionary<string, ExternalGlossary>();

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
        public void LoadManifest(IStatusRegistry registry) {
            addStatus("TempStrafe", "Temp Strafe", "Fire for {0} damage immediately after every move you make. <c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.Violet, System.Drawing.Color.FromArgb(unchecked((int)0xff5e5ce3)), registry, true);
            addStatus("MobileDefense", "Mobile Defense", "Whenever this ship moves, it gains {0} <c=status>TEMP SHIELD</c>. <c=downside>Decreases by 1 at end of turn.</c>",
                true, System.Drawing.Color.Cyan, null, registry, true);
            addStatus("Onslaught", "Onslaught", "Whenever you play a card this turn, draw a card of the <c=keyword>same color</c> from your draw pile." +
                " Decreases by 1 for each card drawn." +
                " <c=downside>Goes away at end of turn.</c>",
                true, System.Drawing.Color.Cyan, null, registry, true);
            // addStatus("Dominance", "Dominance", "Gain {0} <c=status>EVADE</c> each turn. If you don't hit your enemy before your turn ends, <c=downside>lose this status.</c>",
            //     true, System.Drawing.Color.FromArgb(unchecked((int)0x2F48B7)), null, registry, true);
            addStatus("UncannyEvasion", "Damage Control", "Gain {0} <c=status>AUTODODGE</c> if you end your turn without any <c=status>SHIELD</c>.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffff44b6)), null, registry, true);
            addStatus("FalseOpening", "Payback Drive", "Whenever you receieve damage from an attack or missile this turn, " +
                "gain <c=keyword>1</c> <c=status>OVERDRIVE</c> and lower this by <c=keyword>1</c>. " +
                "<c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffff3838)), null, registry, true);
            addStatus("FalseOpeningB", "Payback Powerdrive", "Whenever you receieve damage from an attack or missile this turn, " +
                "gain <c=keyword>1</c> <c=status>POWERDRIVE</c> and lower this by <c=keyword>1</c>. " +
                "<c=downside>Goes away at start of next turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffffd33e)), System.Drawing.Color.FromArgb(unchecked((int)0xffff9e48)), registry, false);
            addStatus("Enflamed", "Enflamed", "Gain {0} <c=downside>HEAT</c> every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffff5660)), System.Drawing.Color.FromArgb(unchecked((int)0xffff5660)), registry, true);
            addStatus("DefensiveStance", "Defensive Stance", "Allows you to play the <c=keyword>top</c> actions on " + JostColH + "Jost's</c> cards. " +
                "<c=downside>If you end your turn with over 1 total stance, lose all stance.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), registry, true);
            addStatus("OffensiveStance", "Offensive Stance", "Allows you to play the <c=keyword>bottom</c> actions on " + JostColH + "Jost's</c> cards. " +
                "<c=downside>If you end your turn with over 1 total stance, lose all stance.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), registry, true);
            addStatus("StandFirm", "Stand Firm", "Prevents you from switching <c=status>STANCE</c> from playing <c=card>STANCE CARDS</c>, " +
                "or losing stance from excess stance. <c=downside>Decreases by 1 at start of turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffffe373)), System.Drawing.Color.FromArgb(unchecked((int)0xffffe373)), registry, true);
            addStatus("Footwork", "Footwork", "Gain {0} <c=status>DEF. STANCE</c> at the start of next turn. " +
                "<c=downside>Decreases by 1 at start of turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), registry, true);
            addStatus("Fortress", "Fortress", "Gain {0} <c=status>TEMP SHIELD</c>, and 1 less <c=downside>ENGINE STALL</c> than you have <c=status>FORTRESS</c> every turn. " +
                "<c=downside>Decreases by 1 every time you move.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffb500be)), System.Drawing.Color.FromArgb(unchecked((int)0xffb500be)), registry, false);
            addStatus("BattleTempo", "Battle Tempo", "Add a <c=cardtrait>temp exhaustable</c> <c=card>Heartbeat</c> to the top of your draw pile every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), System.Drawing.Color.FromArgb(unchecked((int)0xff87b2cf)), registry, false);
            addStatus("DistantStrike", "Distant Strike", "Allows " + ChainColH + "chain lightning</c> to jump between single empty spaces. " +
                "Decreases by 1 for every " + ChainColH + "chain lightning</c> fired.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff00e5ff)), System.Drawing.Color.FromArgb(unchecked((int)0xff00e5ff)), registry, false);
            /*
            addStatus("ElectrocuteCharge", "Electrocute Charge", "Your next " + ChainColH + "chain lightning</c> stuns every part hit. " +
                    "Decreases by 1 for every " + ChainColH + "chain lightning</c> fired.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff00ff40)), System.Drawing.Color.FromArgb(unchecked((int)0xff00ff40)), registry, false);
            addStatus("ElectrocuteSource", "Electrocute Source", "Gain {0} <c=keyword>ELECTROCUTE CHARGE</c> every turn. " +
                "<c=downside>Lose all ELECTROCUTE CHARGE at end of turn.</c>",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffffc47b)), System.Drawing.Color.FromArgb(unchecked((int)0xffffc47b)), registry, true);
            */
            
            addStatus("ElectrocuteCharge", "Electrocute Source", "Your next " + ChainColH + "chain lightning</c> stuns every part hit. " +
                    "Replenishes at end of turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff00ff40)), System.Drawing.Color.FromArgb(unchecked((int)0xff00ff40)), registry, false);
            addStatus("ElectrocuteChargeSpent", "Electrocute Source (Spent)", "Replenishes back into <c=status> ELECTROCUTE CHARGE</c> at end of turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xffffc47b)), System.Drawing.Color.FromArgb(unchecked((int)0xffffc47b)), registry, false);
            
            addStatus("Autocurrent", "Autocurrent", "Automatically shoot a 1 damage " + ChainColH + "chain lightning</c> at the start of every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff00e5ff)), System.Drawing.Color.FromArgb(unchecked((int)0xff00e5ff)), registry, false);
            addStatus("HyperspaceStorm", "Hyperspace Storm", "Gain a <c=card>Hyperspace Wind</c> every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff59f790)), System.Drawing.Color.FromArgb(unchecked((int)0xff59f790)), registry, false);
            addStatus("HyperspaceStormA", "Hyperspace Storm A", "Gain a <c=card>Hyperspace Wind A</c> every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff59f790)), System.Drawing.Color.FromArgb(unchecked((int)0xff59f790)), registry, false);
            addStatus("HyperspaceStormB", "Hyperspace Storm B", "Gain a <c=card>Hyperspace Wind B</c> every turn.",
                true, System.Drawing.Color.FromArgb(unchecked((int)0xff59f790)), System.Drawing.Color.FromArgb(unchecked((int)0xff59f790)), registry, false);

        }
        public void LoadManifest(IGlossaryRegisty registry) {
            addGlossary("EnergyPerCard", "Urgent",
                "This card's cost increases by <c=downside>{0}</c> for every other card played, while in your hand. Resets when played, discarded, or when combat ends."
                , registry);
            addGlossary("EnergyPerCardPerma", "Lasting Urgency",
                "This card's cost increases by <c=downside>{0}</c> for every other card played, while in your hand. Resets when played, or when combat ends."
                , registry);
            addGlossary("EnergyPerPlay", "Rising Cost",
                "This card's cost increases by <c=downside>{0}</c> when played. Resets when discarded, or when combat ends."
                , registry);
            addGlossary("EnergyPerAttack", "Patient",
                "This card's cost decreases by <c=keyword>{0}</c> for every other <c=keyword>attack</c> card " +
                    "played while in your hand. Resets when played, or when combat ends."
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
                "All of this card's values except this one increase by <c=keyword>{0}</c> when played, " +
                    "<c=downside>including cost</c>. Resets when drawn, or when combat ends."
                , registry);
            addGlossary("AllIncreaseCombat", "Lasting Intensify",
                "All of this card's values except this one increase by <c=keyword>{0}</c> when played, " +
                    "<c=downside>including cost</c>. Resets <c=downside>when combat ends.</c>"
                , registry);
            addGlossary("PointDefense", "Point Defense",
                "Align your cannon {0} to the {1} hostile <c=drone>midrow object</c> over your ship. " +
                "If there are none, <c=downside>discard instead</c>. " +
                "Removes <c=cardtrait>retain for this turn when played."
                , registry);
            addGlossary("CallAndResponseHint", "Call and Response",
                "Store the selected card. Whenever you play this card, draw the stored card from the <c=keyword>draw or discard pile</c>{0}.\n" +
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
            addGlossary("DefensiveStanceCost", "Def. Stance Cost",
                "Lose {0} <c=status>DEFENSIVE STANCE</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("OffensiveStanceCost", "Off. Stance Cost",
                "Lose {0} <c=status>OFFENSIVE STANCE</c>. If you don't have enough, this action does not happen."
                , registry);
            addGlossary("DisguisedHint", "Disguised Card",
                "This card may actually be one or more <c=keyword>different</c> kinds of cards, and will not reveal itself until played."
                , registry);
            addGlossary("DisguisedPermaHint", "Permanent Disguise",
                "This card may actually be one or more <c=keyword>different</c> kinds of cards, and <c=downside>will not reveal itself even if played</c>."
                , registry);
            addGlossary("StanceCard", "Stance Card",
                "If you have any <c=status>DEF. STANCE</c>, play the top actions and convert 1 <c=status>DEF. STANCE</c> into " +
                "<c=status>OFF. STANCE</c>. If you have any <c=status>OFF. STANCE</c>, do the opposite."
                , registry);
            addGlossary("ChainLightning", "Chain Lightning",
               "Fires " + Manifest.ChainColH + "lightning</c> from your <c=keyword>drone bay</c>. Travels through " +
                    "<c=midrow>midrow objects</c>, damaging them, <c=midrow>activating drones</c>, and gaining <c=keyword>1</c> damage for each object travelled through. " +
                    " Fires towards the other ship at the end of the chain."
                , registry);
            addGlossary("Conduit", "Conduit",
               "Will block one attack before being destroyed. Takes no damage from " + Manifest.ChainColH + "chain lightning</c>."
                , registry);
            addGlossary("ConduitKinetic", "Kinetic Conduit",
               "Will block one attack before being destroyed. When hit by " + Manifest.ChainColH + "chain lightning</c>, gives 1 <c=status>EVADE</c>."
                , registry);
            addGlossary("ConduitFeedback", "Feedback Conduit",
               "Will block one attack before being destroyed. When hit by " + Manifest.ChainColH + "chain lightning</c>, causes the attacker to " +
                    "shoot another 0 damage " + Manifest.ChainColH + "chain lightning</c>, once per turn."
                , registry);
            addGlossary("ConduitShield", "Shield Conduit",
               "Will block one attack before being destroyed. The first time hit by " + Manifest.ChainColH + "chain lightning</c>, gives 2 <c=status>SHIELD</c> " +
                    "and <c=keyword>bubbles</c> all <c=midrow>midrow objects</c> after it that are not destroyed."
                , registry);
            addGlossary("ConductorField", "Conductor Field",
               "Instantly turn every <c=midrow>object in the midrow</c> into <c=drone>conduits</c>."
                , registry);
        }
    }
}
