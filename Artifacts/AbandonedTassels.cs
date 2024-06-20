using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AbandonedTassels : Artifact {
        public int counter = 0;

        public AbandonedTassels() =>
            Manifest.EventHub.ConnectToEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", StanceSwitch);
        public override string Description() => "The second time you change <c=status>STANCE</c> each turn, gain 1 <c=energy>ENERGY</c>.";

        public override int? GetDisplayNumber(State s) => counter > -1 ? counter : null;

        public override void OnTurnStart(State state, Combat combat) => counter = 0;
        public override void OnCombatStart(State state, Combat combat) => counter = 0;
        public override void OnCombatEnd(State state) => counter = -1;
        
        public override void OnReceiveArtifact(State state) {
            Manifest.EventHub.ConnectToEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", StanceSwitch);
            counter = -1;
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
            if (counter == 2)
                return;
            counter++;
            if (counter == 2) {
                this.Pulse();
                c.Queue(new AEnergy() {
                    changeAmount = 1,
                });
            }
        }
    }
}