using Nickel;
using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class CannonGuard : Artifact {

        public int pos = 0;
        public bool markForGain = false;
        public bool hasMovedThisTurn = false;
        public override string Description() => ManifArtifactHelper.artifactTexts["CannonGuard"];
        public override void OnTurnStart(State state, Combat combat) {
            if (markForGain) {
                markForGain = false;
                combat.Queue(new AStatus() {
                    targetPlayer = true,
                    status = Status.overdrive,
                    statusAmount = 1,
                    dialogueSelector = ".mezz_cannonGuard",
                    artifactPulse = this.Key(),
                });
            }
            pos = state.ship.x;
            hasMovedThisTurn = false;
        }
        public CannonGuard() =>
            Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);

        public override void OnReceiveArtifact(State state) {
            Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
        }
        public override void OnRemoveArtifact(State state) {
            Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
        }

        private void Movement(Tuple<int, bool, bool, Combat, State> evt) {
            State s = evt.Item5;

            if (!s.characters.SelectMany(e => e.artifacts).Concat(s.artifacts).Contains(this)) {
                Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
                return;
            }
            hasMovedThisTurn = hasMovedThisTurn || evt.Item1 != 0;
        }

        public override void OnTurnEnd(State state, Combat combat) {
            markForGain = state.ship.x == pos && hasMovedThisTurn;
        }

        public override void OnCombatStart(State state, Combat combat) {
            markForGain = false;
            hasMovedThisTurn = false;
            pos = state.ship.x;
        }
		public override int? GetDisplayNumber(State s)
		{
			if (pos == s.ship.x && !hasMovedThisTurn) return null;
            return s.ship.x - pos;
		}
		
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1) };
    }
}