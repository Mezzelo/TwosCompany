using System.Runtime.CompilerServices;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class Metronome : Artifact {
        public int counter = 0;
        public bool lastWasMove = false;
        public override string Description() => "Whenever you alternate between moving and attacking <c=keyword>5</c> times in a row, " +
            "gain 1 <c=status>OVERDRIVE</c> and 1 <c=status>EVADE</c>.";

        public Metronome() => Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);

        public override int? GetDisplayNumber(State s) => counter;


        public override void OnRemoveArtifact(State state) {
            Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
            counter = 0;
        }
        // public override void OnCombatEnd(State state) => counter = 0;

        private void Proc(State s, Combat c) {
            counter = 0;
            this.Pulse();
            c.QueueImmediate(new AStatus() {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = 1
            });
            c.QueueImmediate(new AStatus() {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = 1
            });
            foreach (CardAction cardAction in c.cardActions) {
                if (cardAction is AAttack aattack && !aattack.targetPlayer && !aattack.fromDroneX.HasValue)
                    aattack.damage += 1 + s.ship.Get(Status.boost);
            }
        }

        public override void OnPlayerAttack(State state, Combat combat) {
            if (lastWasMove || counter == 0) {
                counter++;
                lastWasMove = false;
                if (counter > 4)
                    Proc(state, combat);
            } else
                counter = 0;
        }

        private void Movement(Tuple<int, bool, bool, Combat, State> evt) {
            int distance = evt.Item1;
            Combat c = evt.Item4;
            State s = evt.Item5;

            if (!s.characters.SelectMany(e => e.artifacts).Concat(s.artifacts).Contains(this)) {
                //make sure cleanup is only performed once.
                Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
                return;
            }

            if (!lastWasMove || counter == 0) {
                counter++;
                lastWasMove = true;
                if (counter > 4)
                    Proc(s, c);
            } else
                counter = 0;
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1), new TTGlossary("status.evade", 1) };
    }
}