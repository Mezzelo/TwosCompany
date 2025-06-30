using System.Runtime.CompilerServices;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class BlackfootPendant : Artifact, IOnMoveArtifact {
        public int counter = 0;
        public override string Description() => ManifArtifactHelper.artifactTexts["BlackfootPendant"];

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            counter = 0;
        }
        public void Movement(int dist, bool targetPlayer, bool fromEvade, Combat c, State s) {
            if (fromEvade)
                return;

            if (counter <= 3 && counter + Math.Abs(dist) > 2) {
                this.Pulse();
                c.QueueImmediate(new AStatus() {
                    targetPlayer = true,
                    status = Status.tempShield,
                    statusAmount = Math.Abs(dist) > 3 ? 2 : 1,
                    dialogueSelector = ".mezz_blackfootPendant",
                });
            }
            counter += Math.Abs(dist);
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.tempShield", 2) };
    }
}