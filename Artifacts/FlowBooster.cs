using TwosCompany.Cards.Nola;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FlowBooster : Artifact {
        public int counter = 0;
        public bool shuffledThisTurn = false;
        public override int? GetDisplayNumber(State s) => counter;

        public override void OnPlayerDeckShuffle(State state, Combat combat) {
            counter = 0;
            shuffledThisTurn = true;
        }
        public override void OnTurnStart(State state, Combat combat) {
            if (counter == 3) {
                counter = 0;
                this.Pulse();
                combat.Queue(new AAddCard() {
                    card = new Foresight() {
                        exhaustOverride = true,
                        temporaryOverride = true,
                        discount = -1
                    },
                    destination = CardDestination.Hand
                });
            }
        }
        public override void OnTurnEnd(State state, Combat combat) {
            if (shuffledThisTurn)
                shuffledThisTurn = false;
            else {
                counter++;
            }
        }

        public override void OnRemoveArtifact(State state) {
            counter = 0;
            shuffledThisTurn = false;
        }

        public override List<Tooltip>? GetExtraTooltips() {
            return new List<Tooltip>() { new TTCard() {
                    card = new Foresight() {
                        exhaustOverride = true,
                        temporaryOverride = true,
                        discount = -1
                    }
                }
            };
        }
    }
}