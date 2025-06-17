using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class TearItAllDown : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["TearItAllDown"];

        public override List<Tooltip>? GetExtraTooltips()
             => new List<Tooltip> { new TTGlossary(Manifest.Glossary["Conduit"]?.Head ??
            throw new Exception("missing glossary entry: Unfreeze")), new TTGlossary("midrow.dualDrone", 1) };
    }
}