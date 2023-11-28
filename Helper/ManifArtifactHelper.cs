using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.Helper {

    public static class ManifArtifactHelper {
        public static String[] artifactNames = new string[] {
            // 0-4: Nola
            "AlphaCore",
            "CommandCenter",
            "FlowBooster",
            "FingerlessGloves",
            "SecondGear",
            // 5-9: Isabelle
            "AncientPhysicalCurrency",
            "BlackfootPendant",
            "AuxiliaryThrusters",
            "CannonGuard",
            "Metronome",
            // 10-14: ilya
            "AncientMatchbox",
            "JerryCan",
            "InciendiaryRounds",
            "ShieldShunt",
            "SleepingPills"
        };

        public static String[] artifactLocs = new string[] {
            // 0-4: Nola
            "Alpha Core",
            "Command Center",
            "Flow Booster",
            "Fingerless Gloves",
            "Second Gear",
            // 5-9: Isabelle
            "Handful of Floren",
            "Crowsfoot Pendant",
            "Auxiliary Thrusters",
            "Cannon Guard",
            "Metronome",
            // 10-14: ilya
            "Worn Lighter",
            "Reactive Coating",
            "Inciendiary Rounds",
            "Shield Shunt",
            "Sleeping Pills"
        };

        public static string[] artifactTexts = new string[] {
            // 0-4: Nola
            "Gain 1 extra <c=energy>ENERGY</c> every turn.\r\n<c=downside>At the start of every combat, exhaust 4 random cards in your draw pile.<c=downside>",
            "Whenever you play <c=keyword>3</c> different colored cards in the same turn, gain 1 <c=energy>ENERGY</c>.",
            "Every <c=keyword>3</c> turns you go without shuffling your deck, gain a free <c=cardtrait>temp exhaustable</c> <c=card>Foresight</c>.",
            "Whenever you play 5 cards in a single turn, gain 1 <c=status>EVADE</c>.",
            "On turn 6, <c=keyword>double</c> all of your current status effects.",
            // 5-9: Isabelle
            "Cards that cost <c=keyword>3+</c> <c=energy>ENERGY</c> are discounted by <c=keyword>1</c> when drawn.\r\n" +
                "Cards that cost <c=keyword>0</c> <c=energy>ENERGY</c> <c=downside>cost 1 more when drawn</c>.",
            "Whenever you move <c=keyword>4+</c> spaces from playing a single card, gain 2 <c=status>TEMP SHIELD</c>.",
            "Choose a card in your deck.  Whenever you play that card, gain a <c=card>Recover</c>.",
            "Whenever you end your turn in the same position you started, gain 1 <c=status>OVERDRIVE</c> next turn.",
            "Whenever you alternate between moving and attacking <c=keyword>5</c> times in a row, gain 1 <c=status>OVERDRIVE</c>.",
            // 10-14: ilya
            "Whenever you <c=downside>overheat</c>, gain 1 <c=status>OVERDRIVE</c>.",
            "At the start of combat, <c=healing>heal 1</c> and <c=downside>gain 2 HEAT</c>.",
            "The first time your enemy is hit each turn, they gain your current <c=keyword>HEAT</c>.",
            "<c=downside>Reduce your max shield to 1</c>, but permanently gain <c=keyword>double</c> your prior max shield as " +
                "<c=hull>max hull</c> and <c=healing>heal</c> the same amount on pickup.",
            "<c=healing>Heal 1</c> and gain 1 <c=status>SERENITY</c> when you <c=downside>overheat</c>, up to once every <c=keyword>4</c> turns."
        };
    }
}
