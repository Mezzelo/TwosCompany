using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using TwosCompany.Cards;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AbandonedTassels : Artifact {
        public int counter = 0;
        public override string Description() => ManifArtifactHelper.artifactTexts["AbandonedTassels"];

        public override int? GetDisplayNumber(State s) => counter > -1 ? counter : null;

        public override void OnTurnStart(State state, Combat combat) => counter = 0;
        public override void OnCombatStart(State state, Combat combat) => counter = 0;
        public override void OnCombatEnd(State state) => counter = -1;

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            if (counter == 2)
                return;
            if (card is IJostCard) {
                counter++;
                if (counter == 2) {
                    this.Pulse();
                    combat.Queue(new AEnergy() {
                        changeAmount = 1,
                    });
                }
            }
        }
    }
}