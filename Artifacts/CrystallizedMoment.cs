using System.Diagnostics.Metrics;
using TwosCompany.Actions;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class CrystallizedMoment : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["CrystallizedMoment"];
        public override void OnTurnEnd(State state, Combat combat) {
            if (state.ship.Get((Status) Manifest.Statuses?["BulletTime"].Id!) >= 2) {
                this.Pulse();
                state.ship.Add((Status)Manifest.Statuses?["BulletTime"].Id!, -1);
                state.ship.Add(Status.timeStop, 1 + state.ship.Get(Status.boost));
                state.ship.Set(Status.boost, 0);
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTGlossary("status." + Manifest.Statuses?["BulletTime"].Id),
            new TTGlossary("status.timeStop", 1),
        };
    }
}