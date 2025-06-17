using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class FieldResonator : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["FieldResonator"];

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            // new TTGlossary(Manifest.Glossary["ChainLightning"]!.Head, 1),
            new TTGlossary("action.attackPiercing")
        };
    }
}