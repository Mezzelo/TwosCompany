using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AncientMatchbox : Artifact {
        public override string Description() => "Whenever you <c=downside>overheat</c>, gain 2 <c=status>OVERDRIVE</c>.";

        public override void AfterPlayerOverheat(State state, Combat combat) {
            this.Pulse();

            combat.Queue(new AStatus() {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = 2
            });
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 2) };
    }
}