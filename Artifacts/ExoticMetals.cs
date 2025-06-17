using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class ExoticMetals : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["ExoticMetals"];
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("midrow.asteroid") };
    }
}