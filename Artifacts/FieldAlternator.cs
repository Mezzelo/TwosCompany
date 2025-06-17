using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FieldAlternator : Artifact {
        public int counter = 0;

        public FieldAlternator() =>
            Manifest.EventHub.ConnectToEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", StanceSwitch);
        public override string Description() => ManifArtifactHelper.artifactTexts["FieldAlternator"];

        public override int? GetDisplayNumber(State s) => counter;

        public override void OnReceiveArtifact(State state) {
            Manifest.EventHub.ConnectToEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", StanceSwitch);
            counter = 0;
        }

        public override void OnRemoveArtifact(State state) {
            Manifest.EventHub.DisconnectFromEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", StanceSwitch);
            counter = 0;
        }

        private void StanceSwitch(Tuple<State, Combat> evt) {
            State s = evt.Item1;
            Combat c = evt.Item2;
            if (!s.characters.SelectMany(e => e.artifacts).Concat(s.artifacts).Contains(this)) {
                Manifest.EventHub.DisconnectFromEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", StanceSwitch);
                return;
            }

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