using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class PressureReservoir : Artifact {
        public override string Description() => "Whenever you lose 3 or more <c=status>HEAT</c> at once, gain 2 <c=status>HEAT</c>.";

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1) };
    }
}