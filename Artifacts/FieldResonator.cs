namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class FieldResonator : Artifact {
        public override string Description() => Manifest.ChainColH + "Chain lightning</c> now <c=keyword>pierces</c>, ignoring armor, shields and bubbles.";

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            // new TTGlossary(Manifest.Glossary["ChainLightning"]!.Head, 1),
            new TTGlossary("action.attackPiercing")
        };
    }
}