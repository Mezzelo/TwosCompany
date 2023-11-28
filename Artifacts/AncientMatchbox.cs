namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AncientMatchbox : Artifact {

        public override void AfterPlayerOverheat(State state, Combat combat) {
            this.Pulse();

            combat.Queue(new AStatus() {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = 1
            });
        }
    }
}