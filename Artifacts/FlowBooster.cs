using System.Collections.Generic;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FlowBooster : Artifact {
        public int counter = 0;
        public bool shuffledThisTurn = false;
        public override string Description() => "Every <c=keyword>3</c> turns you go without shuffling your deck, " +
            "gain a free <c=cardtrait>temp exhaustable</c> <c=card>Foresight</c>.";
        public override int? GetDisplayNumber(State s) => counter;

        public override void OnPlayerDeckShuffle(State state, Combat combat) {
            counter = 0;
            shuffledThisTurn = true;
        }
        public override void OnTurnStart(State state, Combat combat) {
            if (counter == 3) {
                counter = 0;
                combat.Queue(new AAddCard() {
                    card = new Foresight() {
                        exhaustOverride = true,
                        temporaryOverride = true,
                        discount = -1,
                    },
                    destination = CardDestination.Hand,
                    artifactPulse = this.Key(),
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
            return new List<Tooltip>() { new TTGlossary("action.addCard", "<c=deck>hand</c>"),
                    new TTCard() {
                    card = new Foresight() {
                        exhaustOverride = true,
                        temporaryOverride = true,
                        discount = -1
                    },
                    showCardTraitTooltips = false
                }
            };
        }
    }
}