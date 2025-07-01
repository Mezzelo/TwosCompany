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
            // 0-6: Nola
            { "AlphaCore", "Alpha Core"},
            { "BufferEcho", "Buffer Echo"},
            { "CommandCenter", "Command Center"},
            { "FlowBooster", "Flow Booster"},
            { "FingerlessGloves", "Fingerless Gloves"},
            { "SecondGear", "Second Gear"},
            { "VestigeOfHumanity", "Vestige of Conscience"},
            // 7-13: Isabelle
            { "AncientPhysicalCurrency", "Handful of Floren"},
            { "BlackfootPendant", "Crowsfoot Pendant"},
            { "AuxiliaryThrusters", "Auxiliary Thrusters"},
            { "CannonGuard", "Cannon Guard"},
            { "FlawlessCore", "Flawless Core"},
            { "Metronome", "Metronome"},
            { "LongLostRegrets", "Long Lost Regrets"},
            // 14-20: ilya
            { "AncientMatchbox", "Worn Lighter"},
            { "JerryCan", "Reactive Coating"},
            { "IncendiaryRounds", "Incendiary Rounds"},
            { "PressureReservoir", "Pressure Reservoir"},
            { "ShieldShunt", "Shield Shunt"},
            { "SleepingPills", "Sleeping Pills"},
            { "EternalFlame", "Eternal Flame"},
            // 21-28: jost
            { "CrumpledWrit", "Crumpled Writ"},
            { "MessengerBag", "Messenger's Bag"},
            { "MilitiaArmband", "Militia Armband"},
            { "FieldAlternator", "Field Alternator"},
            { "BlackfootSigil", "Crowsfoot Sigil"},
            { "BurdenOfHindsight", "Burden of Hindsight"},
            { "AbandonedTassels", "Frayed Tassels"},
            { "AimlessVengeance", "Aimless Vengeance"},
            // 29-35: gauss
            { "TwinMaleCable", "Twin Male Cable"},
            { "IonEngines", "Ion Engines"},
            { "ExoticMetals", "Exotic Metals"},
            { "FieldResonator", "Shield Current Resonator"},
            { "RemoteStarter", "Conduit Condenser"},
            { "TuningTrident", "Tuning Trident"},
            { "TearItAllDown", "Tear It All Down"},
            // 36-41: sorrel
            { "AmberedThoughts", "Ambered Thoughts"},
            { "BlackfootSymbol", "Blackfoot Symbol"},
            { "CrystallizedMoment", "Crystallized Moment"},
            { "EternalIncense", "Endless Incense"},
            { "ShrinkingHourglass", "Shrinking Hourglass"},
            { "VoidScripture", "Void Mantra"},
            { "Ascension", "Ascension"},
        };

        public static Dictionary<string, string> artifactTexts = new Dictionary<string, string> {
            { "AlphaCore",
                "Gain 1 extra <c=energy>ENERGY</c> every turn.\r\nAt the start of every combat," +
                " <c=downside>exhaust 4 random cards in your draw pile.<c=downside>"},
            { "BufferEcho",
                "Choose a card in your deck. Whenever you play that card, " +
                "<c=cardtrait>exhaust</c> it and gain a <c=cardtrait>temp</c> copy of it." },
            { "CommandCenter",
                "Whenever you play <c=keyword>3</c> different colored cards in the same turn, gain 1 <c=energy>ENERGY</c>."},
            { "FlowBooster",
                "Every <c=keyword>3</c> turns you go without shuffling your deck, gain a free <c=cardtrait>temp exhaustable</c> <c=card>Foresight</c>."},
            { "FingerlessGloves",
                "Whenever you play 5 cards in a single turn, gain 1 <c=status>EVADE</c>."},
            { "SecondGear",
                "At the start of turn 6, increase all of your current <c=status>statuses</c> by <c=keyword>1</c>."},
            { "VestigeOfHumanity",
                "At the start of each turn, gain 1 <c=status>ONSLAUGHT</c>."},
            // 7-13: Isabelle
            { "AncientPhysicalCurrency",
                "The first two times you draw a 2+ cost card each turn, discount it by 1.\r\n" +
                "<c=downside>The first time you draw a non-discounted card that costs 1 or less each turn, increase its cost by 1.</c>"},
            { "BlackfootPendant",
            "Whenever you move <c=keyword>3</c> spaces from playing a single card, gain 1 <c=status>TEMP SHIELD</c>." +
                " If you move <c=keyword>4+</c> spaces, gain another."},
            { "AuxiliaryThrusters",
            "Choose a card in your deck.  Whenever you play that card, gain a <c=card>Recover</c>."},
            { "CannonGuard",
                "Whenever you end your turn in the same position you started it in after having moved," +
                " gain 2 <c=status>TEMP SHIELD</c>, and gain 1 <c=status>OVERDRIVE</c> next turn."},
            { "FlawlessCore",
                "Gain 1 extra <c=energy>ENERGY</c> every turn. " +
                "<c=downside>If you missed a shot during your previous turn, lose 1 ENERGY instead.</c>"},
            { "Metronome",
                "Whenever you alternate between attacking and moving using <c=status>EVADE</c> <c=keyword>6</c> times in a row," +
                " gain 1 <c=status>OVERDRIVE</c> and 2 <c=status>EVADE</c>. Each card played cannot add more than 2 stacks at a time."},
            { "LongLostRegrets",
            "Gain 1 extra <c=energy>ENERGY</c> every turn. " +
                "<c=downside>The first time you receive hull damage each turn, gain two</c> <c=card>Fears</c>."},
            // 14-20: ilya
            { "AncientMatchbox",
                "Whenever you <c=downside>overheat</c>, gain 1 <c=status>OVERDRIVE</c> and 1 <c=status>HEAT</c>."},
            { "JerryCan",
                "At the start of combat, <c=healing>heal 1</c> and <c=downside>gain 2 HEAT</c>." },
            { "IncendiaryRounds",
                "Whenever you hit your enemy, they gain your current <c=status>HEAT</c>. If they would <c=downside>overheat</c>, they do so immediately."},
            { "PressureReservoir",
                "Whenever you start your turn with 2+ <c=status>HEAT</c>, convert 1 into <c=status>HEAT FEEDBACK</c>."},
            { "ShieldShunt",
                "On pickup, <c=downside>reduce your max shield to 1</c>, but permanently gain <c=keyword>triple</c> your prior max shield as " +
                "<c=hull>max hull</c> and <c=healing>heal</c> the same amount."},
            { "SleepingPills",
                "<c=healing>Heal 1</c> and gain 1 <c=status>SERENITY</c> whenever you <c=downside>overheat</c>, up to once every <c=keyword>4</c> turns."},
            { "EternalFlame",
                "Start each combat with 2 <c=status>ENFLAMED</c>."},
            // 21-28: jost
            { "CrumpledWrit",
                "Start each combat with 1 <c=status>DEF. STANCE</c>. " +
                "When you start your turn with no stance, gain an <c=card>Off Balance</c> if one isn't in your hand."},
            { "MessengerBag",
                "On pickup, gain 2 <c=card>Heartbeats</c>. The first time you draw a <c=card>Heartbeat</c> each turn, gain 1 <c=status>ONSLAUGHT</c>."},
            { "MilitiaArmband",
                 "Once per turn when you increase your total <c=status>STANCE</c>, " +
                "gain 1 <c=status>OVERDRIVE</c>." },
            { "FieldAlternator",
                "Every <c=keyword>4</c> times you switch stance from playing a " +
                "<c=card>STANCE CARD</c>, gain a <c=cardtrait>temp exhaustable</c> <c=card>Heartbeat</c>."},
            { "BlackfootSigil",
                "At the start of combat, gain a <c=card>Freedom of Movement</c>."},
            { "BurdenOfHindsight",
                "Increases damage dealt by stacks of <c=status>OFF. STANCE</c>," +
                " and <c=card>Off Balance</c> costs 1 less energy. <c=downside>If you end your turn with any DEF. STANCE, lose 1 of each stance.</c>"},
            { "AbandonedTassels",
                "The second time you play a <c=card>STANCE CARD</c> each turn, gain 1 <c=energy>ENERGY</c>."},
            { "AimlessVengeance",
                "Start each combat with 3 <c=status>OFF. STANCE</c>."},
            // 29-35: gauss
            { "TwinMaleCable",
                "+1 " + Manifest.ChainColH + "chain lightning</c> damage. " +
                "Whenever you split " + Manifest.ChainColH + "chain lightning</c>, " +
                "combine the split damage into the <c=redd>right end</c> instead of firing from the <c=attackFail>left end</c>."},
            { "IonEngines",
                "For every 5 <c=midrow>midrow objects</c> you " + Manifest.ChainColH + "chain</c> through, gain 1 <c=status>EVADE</c>."},
            { "ExoticMetals",
                Manifest.ChainColH + "Chain lightning</c> no longer damages <c=midrow>asteroids</c>. " +
                "Non-conduit/asteroid midrow objects increase chain damage by <c=keyword>2</c> instead of <c=keyword>1</c>."},
            { "FieldResonator",
                Manifest.ChainColH + "Chain lightning</c> now <c=keyword>pierces</c> armor, shields and bubbles."},
            { "RemoteStarter",
                "For every 4 <c=midrow>midrow objects</c> you destroy, gain a free <c=cardtrait>temp exhaustable</c> <c=card>Conduit</c>, up to " +
                "once per turn."},
            { "TuningTrident",
                "When you " + Manifest.ChainColH + "chain</c> across 2 or more <c=midrow>midrow objects</c>, " + Manifest.ChainColH +
                "lightning</c> fires from the start of the chain <c=downside>at a -2 damage penalty.</c>"},
            { "TearItAllDown",
                "Whenever you launch a regular <c=midrow>conduit</c>, gain 1 <c=status>SHIELD</c>, " +
                "1 <c=status>TEMP SHIELD</c>, and turn it into a <c=downside>dual drone</c> instead."},
            // 36-41: sorrel
            { "AmberedThoughts",
                "Choose a card in your deck. The first time you play it each turn, gain 1 <c=status>DRONESHIFT</c>. " +
                "You also gain 1 <c=status>BULLET TIME</c> if you do not have any."},
            { "BlackfootSymbol",
                "At the start of each turn, gain 1 <c=status>DRONESHIFT</c> if you have 2 or less and" +
                " there are any " + Manifest.FrozenColH + "frozen attacks</c>."},
            { "CrystallizedMoment",
                "Whenever you end your turn with 2+ <c=status>BULLET TIME</c>, convert 1 into <c=status>TIMESTOP</c>."},
            { "EternalIncense",
                "<c=downside>ALL</c> attacks deal +1 damage while <c=status>BULLET TIME</c> is active."},
            { "ShrinkingHourglass",
                "Whenever you end your turn with <c=status>BULLET TIME</c>, fire a 0 damage attack."},
            { "VoidScripture",
                "Whenever you end your turn with 2+ <c=status>BULLET TIME</c>, gain 1 <c=statuc>TEMP SHIELD</c> and 1 <c=status>TEMP PAYBACK</c>."},
            { "Ascension",
                "At the start of turn 4, gain an <c=cardtrait>infinite retain</c> <c=card>Karma B</c>. " +
                "The turn count increases by 2 for each additional crew member aboard the Artemis."},
        };
    }
}
