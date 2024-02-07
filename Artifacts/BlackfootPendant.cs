using System.Runtime.CompilerServices;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class BlackfootPendant : Artifact {
        public BlackfootPendant() => Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);

        public int counter = 0;
        public override string Description() => "Whenever you move <c=keyword>3</c> spaces from playing a single card, gain 1 <c=status>TEMP SHIELD</c>. " +
            "If you move <c=keyword>4+</c> spaces, gain another.";

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
            if (counter <= 3 && counter + Math.Abs(distance) > 2) {
                this.Pulse();
                c.QueueImmediate(new AStatus() {
                    targetPlayer = true,
                    status = Status.tempShield,
                    statusAmount = Math.Abs(distance) > 3 ? 2 : 1,
                    dialogueSelector = ".mezz_blackfootPendant",
                });
            }
            counter += Math.Abs(distance);
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.tempShield", 2) };
    }
}