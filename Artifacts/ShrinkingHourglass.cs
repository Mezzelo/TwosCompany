using System.Diagnostics.Metrics;
using TwosCompany.Actions;
using TwosCompany.Helper;
using TwosCompany.Midrow;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class ShrinkingHourglass : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["ShrinkingHourglass"];

        public override void OnTurnEnd(State state, Combat combat) {
            if (state.ship.Get((Status) Manifest.Statuses?["BulletTime"].Id!) > 0) {
                combat.QueueImmediate(new AAttack() {
                    targetPlayer = false,
                    damage = Card.GetActualDamage(state, 0, false, null),
                    artifactPulse = this.Key(),
                    dialogueSelector = ".mezz_shrinkingHourglass",
                });
            }
        }
        public override void OnTurnStart(State state, Combat combat) {
            if (state.ship.Get(Status.droneShift) >= 2)
                return;
            bool found = false;
            foreach (StuffBase stuff in combat.stuff.Values) {
                if (stuff is FrozenAttack fAttack) {
                    found = true;
                    break;
                }
            }
            if (found) {
                combat.Queue(new AStatus() {
                    status = Status.droneShift,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = this.Key()
                });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTGlossary("status." + Manifest.Statuses?["BulletTime"].Id),
        };
    }
}