using System;
using System.Diagnostics.Metrics;
using TwosCompany;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class CommandCenter : Artifact {
        public override string Description() => "Whenever you play <c=keyword>3</c> different colored cards in the same turn, gain 1 <c=energy>energy</c>.";
        public List<Deck> decks = new List<Deck>();
        public int count = 0;
        public override int? GetDisplayNumber(State s) => count;
        public override void OnTurnEnd(State state, Combat combat) {
            decks.Clear();
            count = 0;
        }
        public override void OnCombatEnd(State state) {
            decks.Clear();
            count = 0;
        }
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            if (count < 3) {
                if (!decks.Contains(card.GetMeta().deck)) {
                    decks.Add(card.GetMeta().deck);
                    count++;
                }
                if (count == 3) {
                    this.Pulse();
                    combat.Queue(new AEnergy() {
                        changeAmount = 1,
                    });
                }
            }
        }
    }
}