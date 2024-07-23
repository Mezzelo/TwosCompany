namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class TearItAllDown : Artifact {
        public override string Description() =>
            "Whenever you launch a regular <c=midrow>conduit</c>, gain 1 <c=status>SHIELD</c>, " +
                "1 <c=status>TEMP SHIELD</c>, and turn it into a <c=downside>dual drone</c> instead.";

        public override List<Tooltip>? GetExtraTooltips()
             => new List<Tooltip> { new TTGlossary(Manifest.Glossary["Conduit"]?.Head ??
            throw new Exception("missing glossary entry: Unfreeze")), new TTGlossary("midrow.dualDrone", 1) };
    }
}