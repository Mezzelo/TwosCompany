using Nickel;
using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class CannonGuard : Artifact, IOnMoveArtifact {

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

        public void Movement(int dist, bool targetPlayer, bool fromEvade, Combat c, State s) {
            hasMovedThisTurn = hasMovedThisTurn || dist != 0;
        }

        public override void OnTurnEnd(State state, Combat combat) {
            if (state.ship.x == pos && hasMovedThisTurn) {
                markForGain = true;
                combat.Queue(new AStatus() {
                    targetPlayer = true,
                    status = Status.tempShield,
                    statusAmount = 2,
                    artifactPulse = this.Key(),
                });
            }
        }

        public override void OnCombatStart(State state, Combat combat) {
            markForGain = false;
            hasMovedThisTurn = false;
            pos = state.ship.x;
        }
		public override int? GetDisplayNumber(State s)
		{
			if (!(s.route is Combat) || pos == s.ship.x && !hasMovedThisTurn) return null;
            return s.ship.x - pos;
		}
		
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { 
            new TTGlossary("status.overdrive", 1),
            new TTGlossary("status.tempShield", 1),
        };
    }
}