namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class TuningTrident : Artifact {
        public override string Description() => Manifest.ChainColH + "Chain lightning</c> always fires through the first " +
                "<c=midrow</c>midrow object</c> it hits, <c=downside>at a -1 damage penalty.</c>";


    }
}