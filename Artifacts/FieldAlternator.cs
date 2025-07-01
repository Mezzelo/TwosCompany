using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FieldAlternator : Artifact, IOnStanceSwitchArtifact {
        public int counter = 0;
        public override string Description() => ManifArtifactHelper.artifactTexts["FieldAlternator"];

        public override int? GetDisplayNumber(State s) => counter;

        public void StanceSwitch(State s, Combat c) {
            counter++;
            if (counter == 4) {
                counter = 0;
                this.Pulse();
                c.Queue(new AAddCard() {
                    card = new Heartbeat() { exhaustOverride = true, temporaryOverride = true },
                    destination = CardDestination.Hand,
                });
            }
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTCard() {
                card = new Heartbeat() { exhaustOverride = true, temporaryOverride = true },
            }
        };
    }
}