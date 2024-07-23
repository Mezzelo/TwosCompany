using System.Diagnostics.Metrics;
using TwosCompany.Actions;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class VoidScripture : Artifact {
        public override string Description() => 
            "Whenever you end your turn with 2+ <c=status>BULLET TIME</c>, gain 1 <c=statuc>TEMP SHIELD</c> and 1 <c=status>TEMP PAYBACK</c>.";
        public override void OnTurnEnd(State state, Combat combat) {
            if (state.ship.Get((Status) Manifest.Statuses?["BulletTime"].Id!) >= 2) {
                combat.Queue(new AStatus() {
                    status = Status.tempPayback,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = this.Key()
                });
                combat.Queue(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = this.Key()
                });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTGlossary("status." + Manifest.Statuses?["BulletTime"].Id),
            new TTGlossary("status.tempShield", 1),
            new TTGlossary("status.tempPayback", 1),
        };
    }
}