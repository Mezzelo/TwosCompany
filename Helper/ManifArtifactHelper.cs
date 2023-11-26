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
            "AuxiliaryThrusters",
            "BlackfootPendant",
            "CannonGuard",
            "Metronome",
            // 10-14: ilya
            "AncientMatchbox",
            "JerryCan",
            "Reactor Accelerant",
            "ShieldShunt",
            "SleepingPills"
        };

        public static String[] artifactLocs = new string[] {
            // 0-4: Nola
            "Alpha Core",
            "Command Center",
            "Flow Booster",
            "FingerlessG loves",
            "Second Gear",
            // 5-9: Isabelle
            "Ancient Physical Currency",
            "Auxiliary Thrusters",
            "Blackfoot Pendant",
            "Cannon Guard",
            "Metronome",
            // 10-14: ilya
            "Ancient Matchbox",
            "Jerry Can",
            "Reactor Accelerant",
            "Shield Shunt",
            "Sleeping Pills"
        };

        public static string[] artifactTexts = new string[] {
            // 0-4: Nola
            "Gain 1 extra <c=energy>ENERGY</c> every turn.\r\n<c=downside>At the start of every combat, exhaust 5 random cards in your draw pile.<c=downside>",
            "Draw a different colored card every time you spend 3+ <c=energy>energy</c> playing a card.",
            "Every <c=keyword>3</c> turns you go without shuffling your deck, gain a free <c=cardtrait>temp exhaustable</c> <c=card>Foresight</c>.",
            "Whenever you play 5 cards in a single turn, gain 1 <c=status>EVADE</c>.",
            "On turn 6, double all of your status effects.",
            // 5-9: Isabelle
            "Cards that cost 4 or more energy cost 1 less when drawn. Cards that cost 0 energy cost 1 more when drawn.",
            "Select one of Isabelle's cards.  Whenever you draw that card, add a readjust to your hand\r\n> 2, flippable.  0 cost, rising cost, retain, temp & exhaust.",
            "Blackfoot Pendant",
            "Cannon Guard",
            "Metronome",
            // 10-14: ilya
            "Ancient Matchbox",
            "Jerry Can",
            "Reactor Accelerant",
            "<c=downside>Reduce your max shield to 1</c>, but permanently gain <c=keyword>double</c> your prior max shield as " +
            "<c=hull>max hull</c> and <c=healing>heal</c> the same amount on pickup.",
            "Once per <c=keyword>4</c> turns when you overheat, <c=healing>heal 1</c> and gain 1 <c=status>serenity</c>."
        };
    }
}
