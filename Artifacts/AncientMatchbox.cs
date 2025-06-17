using TwosCompany.Cards.Isabelle;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AncientMatchbox : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["AncientMatchbox"];

        public override void AfterPlayerOverheat(State state, Combat combat) {
            this.Pulse();

            combat.Queue(new AStatus() {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = 1,
            });

            combat.Queue(new AStatus() {
                targetPlayer = true,
                status = Status.heat,
                statusAmount = 1
            });
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1) };
    }
}