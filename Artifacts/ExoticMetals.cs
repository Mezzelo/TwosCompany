namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class ExoticMetals : Artifact {
        public override string Description() => Manifest.ChainColH + "Chain lightning</c> no longer damages <c=midrow>asteroids</c>. " +
                " Non-missile, active objects increase chain damage by <c=keyword>2</c> instead of <c=keyword>1</c>.";
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("midrow.asteroid") };
    }
}