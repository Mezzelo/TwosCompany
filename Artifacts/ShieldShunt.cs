using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class ShieldShunt : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["ShieldShunt"];

        public override void OnReceiveArtifact(State state) {

            int increase = state.ship.shieldMaxBase * 3;
            state.ship.hullMax += increase;
            state.ship.hull += increase;
            state.ship.shieldMaxBase = 1;
        }

        public override void OnRemoveArtifact(State state) {
        }
    }
}