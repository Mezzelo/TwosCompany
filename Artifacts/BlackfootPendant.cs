using System.Runtime.CompilerServices;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class BlackfootPendant : Artifact {
        public BlackfootPendant() => Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);

        public int counter = 0;

        public override void OnRemoveArtifact(State state) => Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            counter = 0;
        }
        private void Movement(Tuple<int, bool, bool, Combat, State> evt) {
            int distance = evt.Item1;
            bool targetPlayer = evt.Item2;
            bool fromEvade = evt.Item3;
            Combat c = evt.Item4;
            State s = evt.Item5;

            if (!s.characters.SelectMany(e => e.artifacts).Concat(s.artifacts).Contains(this)) {
                //make sure cleanup is only performed once.
                Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
                return;
            }

            if (fromEvade)
                return;
            if (counter <= 3 && counter + Math.Abs(distance) > 3) {
                this.Pulse();
                c.QueueImmediate(new AStatus() {
                    targetPlayer = true,
                    status = Status.tempShield,
                    statusAmount = 2
                });
            }
            counter += Math.Abs(distance);
        }
    }
}