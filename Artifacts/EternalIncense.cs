using CobaltCoreModding.Definitions.ExternalItems;
using System.Diagnostics.Metrics;
using TwosCompany.Actions;
using TwosCompany.Midrow;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class EternalIncense : Artifact {
        public override string Description() => "<c=downside>ALL</c> attacks deal +1 damage while <c=status>BULLET TIME</c> is active.";
        public override void OnEnemyAttack(State state, Combat combat) {
            if (state.ship.Get((Status)Manifest.Statuses?["BulletTime"].Id!) > 0 ||
                combat.otherShip.Get((Status)Manifest.Statuses?["BulletTime"].Id!) > 0)
                this.Pulse();
        }
        public override void OnPlayerAttack(State state, Combat combat) {
            if (state.ship.Get((Status)Manifest.Statuses?["BulletTime"].Id!) > 0 ||
                combat.otherShip.Get((Status)Manifest.Statuses?["BulletTime"].Id!) > 0)
                this.Pulse();
        }
        public override int ModifyBaseDamage(
          int baseDamage,
          Card? card,
          State state,
          Combat? combat,
          bool fromPlayer) {
            if (combat == null)
                return 0;
            if (state.ship.Get((Status)Manifest.Statuses?["BulletTime"].Id!) > 0 ||
                combat.otherShip.Get((Status)Manifest.Statuses?["BulletTime"].Id!) > 0)
                return 1;
            return 0;
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTGlossary("status." + Manifest.Statuses?["BulletTime"].Id),
        };
    }
}