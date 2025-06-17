using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class TuningTrident : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["TuningTrident"];


    }
}