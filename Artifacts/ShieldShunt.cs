namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class ShieldShunt : Artifact {
        public override string Description() => "On pickup, <c=downside>reduce your max shield to 1</c>, but permanently gain <c=keyword>double</c> your prior max shield as " +
            "<c=hull>max hull</c> and <c=healing>heal</c> the same amount.";

        public override void OnReceiveArtifact(State state) {

            int increase = state.ship.shieldMaxBase * 2;
            state.ship.hullMax += increase;
            state.ship.hull += increase;
            state.ship.shieldMaxBase = 1;
        }

        public override void OnRemoveArtifact(State state) {
        }
    }
}