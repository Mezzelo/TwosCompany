using System;
using TwosCompany.Artifacts;
using TwosCompany.Helper;
using TwosCompany.Zones;


namespace TwosCompany {
    public static class StoryChoicesTC {
        public static List<Choice> MemChoiceNola(State s) {
            return new List<Choice> { new Choice {
                label = "Take it.",
                key = "mezz_Sorrel_Memory_2_Start",
                actions = { new AAddArtifact {
                artifact = new VestigeOfHumanity()
            }}}};
        }
        public static List<Choice> MemChoiceIsa(State s) {
            return new List<Choice> { new Choice {
                label = "Take it.",
                key = "mezz_Jost_Memory_3_Start",
                actions = { new AAddArtifact {
                artifact = new LongLostRegrets()
            }}}};
        }
        public static List<Choice> MemChoiceIlya(State s) {
            return new List<Choice> { new Choice {
                label = "Take it.",
                key = "mezz_Jost_Memory_1_Start",
                actions = { new AAddArtifact {
                artifact = new EternalFlame()
            }}}};
        }
        public static List<Choice> MemChoiceJost(State s) {
            return new List<Choice> { new Choice {
                label = "Take it.",
                key = "mezz_Sorrel_Memory_1_Start",
                actions = { new AAddArtifact {
                artifact = new AimlessVengeance()
            }}}};
        }
        public static List<Choice> MemChoiceGauss(State s) {
            return new List<Choice> { new Choice {
                label = "Take it.",
                key = "mezz_Jost_Memory_2_Start",
                actions = { new AAddArtifact {
                artifact = new TearItAllDown()
            }}}};
        }
    }
}
