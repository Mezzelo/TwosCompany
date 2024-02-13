using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TwosCompany.Artifacts;
using TwosCompany.Cards;

namespace TwosCompany.Helper {

    public static class ManifArtifactHelper {
        public static Dictionary<string, string> artifactNames = new Dictionary<string, string> {
            // 0-4: Nola
            { "AlphaCore", "Alpha Core"},
            { "CommandCenter", "Command Center"},
            { "FlowBooster", "Flow Booster"},
            { "FingerlessGloves", "Fingerless Gloves"},
            { "SecondGear", "Second Gear"},
            // 5-9: Isabelle
            { "AncientPhysicalCurrency", "Handful of Floren"},
            { "BlackfootPendant", "Crowsfoot Pendant"},
            { "AuxiliaryThrusters", "Auxiliary Thrusters"},
            { "CannonGuard", "Cannon Guard"},
            { "Metronome", "Metronome"},
            // 10-14: ilya
            { "AncientMatchbox", "Worn Lighter"},
            { "JerryCan", "Reactive Coating"},
            { "IncendiaryRounds", "Incendiary Rounds"},
            { "ShieldShunt", "Shield Shunt"},
            { "SleepingPills", "Sleeping Pills"},
            // 15-20: jost
            { "CrumpledWrit", "Crumpled Writ"},
            { "MessengerBag", "Messenger's Bag"},
            { "MilitiaArmband", "Militia Armband"},
            { "FieldAlternator", "Field Alternator"},
            { "BlackfootSigil", "Crowsfoot Sigil"},
            { "BurdenOfHindsight", "Burden of Hindsight"},
            // 21-25: gauss
            { "TwinMaleCable", "Twin Male Cable"},
            { "IonEngines", "Ion Engines"},
            { "ExoticMetals", "Exotic Metals"},
            { "FieldResonator", "Shield Current Resonator"},
            { "RemoteStarter", "Conduit Condenser"},
        };

        public static string[] artifactTexts = new string[] {
            // 0-4: Nola
            "Gain 1 extra <c=energy>ENERGY</c> every turn.\r\nAt the start of every combat, <c=downside>exhaust 4 random cards in your draw pile.<c=downside>",
            "Whenever you play <c=keyword>3</c> different colored cards in the same turn, gain 1 <c=energy>ENERGY</c>.",
            "Every <c=keyword>3</c> turns you go without shuffling your deck, gain a free <c=cardtrait>temp exhaustable</c> <c=card>Foresight</c>.",
            "Whenever you play 5 cards in a single turn, gain 1 <c=status>EVADE</c>.",
            "At the start of turn 6, <c=keyword>double</c> all of your current status effects.",
            // 5-9: Isabelle
            "Cards that cost <c=keyword>2</c> or <c=keyword>4</c> <c=energy>ENERGY</c> are discounted by <c=keyword>1</c> when drawn.\r\n" +
                "Cards that cost <c=keyword>0</c> <c=energy>ENERGY</c> <c=downside>cost 1 more when drawn</c>.",
            "Whenever you move <c=keyword>3</c> spaces from playing a single card, gain 1 <c=status>TEMP SHIELD</c>. If you move <c=keyword>4+</c> spaces, gain another.",
            "Choose a card in your deck.  Whenever you play that card, gain a <c=card>Recover</c>.",
            "Whenever you end your turn in the same position you started, gain 1 <c=status>OVERDRIVE</c>.",
            "Whenever you alternate between moving and attacking <c=keyword>6</c> times in a row, gain 1 <c=status>OVERDRIVE</c> and 1 <c=status>EVADE</c>.",
            // 10-14: ilya
            "Whenever you <c=downside>overheat</c>, gain 1 <c=status>OVERDRIVE</c> and 1 <c=downside>HEAT</c>.",
            "At the start of combat, <c=healing>heal 1</c> and <c=downside>gain 2 HEAT</c>.",
            "Whenever you hit your enemy, they gain your current <c=status>HEAT</c>. If they would <c=downside>overheat</c>, they do so immediately.",
            "<c=downside>Reduce your max shield to 1</c>, but permanently gain <c=keyword>double</c> your prior max shield as " +
                "<c=hull>max hull</c> and <c=healing>heal</c> the same amount on pickup.",
            "<c=healing>Heal 1</c> and gain 1 <c=status>SERENITY</c> whenever you <c=downside>overheat</c>, up to once every <c=keyword>4</c> turns.",
            // 15-20: jost
            "Start each combat with 1 <c=status>DEF. STANCE</c>. " +
                "When you start your turn with no stance, gain an <c=card>Off Balance</c> if one isn't in your hand.",
            "Gain 3 <c=status>ONSLAUGHT</c> every turn. <c=downside>Draw 1 less card per turn.</c>",
            "Once per turn when you increase your total <c=status>OFF.</c> and <c=status>DEF. STANCE</c>, " +
                "gain 1 <c=status>OVERDRIVE</c>.",
            "Every <c=keyword>4</c> times you switch stance from playing a " +
                "<c=card>STANCE CARD</c>, gain a <c=cardtrait>temp exhaustable</c> <c=card>Heartbeat</c>.",
            "Choose a card in your deck. Whenever you play that card, <c=status>switch stance</c> and gain 1 <c=status>SHIELD</c>. " +
                "Switch stance an additional time if it is already a <c=card>STANCE CARD</c>.",
            "All " + Manifest.JostColH + "Jost</c> cards deal 1 more damage while you have <c=status>OFF. STANCE</c>," +
                " and <c=card>Off Balance</c> costs 1 less energy. <c=downside>If you end your turn with any DEF. STANCE, lose all stance.</c>",
            // 21-25: gauss
            "<c=keyword>+1</c> " + Manifest.ChainColH + "chain lightning</c> damage. " +
                "Whenever you split " + Manifest.ChainColH + "chain lightning</c>, " +
                "combine the split damage into the <c=redd>right end</c> instead of firing from the <c=attackFail>left end</c>.",
            "For every 5 <c=midrow>midrow objects</c> you " + Manifest.ChainColH + "chain</c> through, gain 1 <c=status>EVADE</c>.",
            Manifest.ChainColH + "Chain lightning</c> no longer damages <c=midrow>asteroids</c>. " +
                " Non-missile, active objects increase chain damage by <c=keyword>2</c> instead of <c=keyword>1</c>.",
            Manifest.ChainColH + "Chain lightning</c> now <c=keyword>pierces</c> armor, shields and bubbles.",
            "For every 4 <c=midrow>midrow objects</c> you destroy, gain a free <c=cardtrait>temp exhaustable</c> <c=card>Conduit</c>, up to" +
                "once per turn.",
        };
    }
}
