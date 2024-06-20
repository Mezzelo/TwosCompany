using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AncientMatchbox : Artifact {
        public override string Description() => "Whenever you <c=downside>overheat</c>, gain 1 <c=status>OVERDRIVE</c> and 1 <c=status>HEAT</c>.";

        public override void AfterPlayerOverheat(State state, Combat combat) {
            this.Pulse();

            combat.Queue(new AStatus() {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = 1,
            });

            combat.Queue(new AStatus() {
                targetPlayer = true,
                status = Status.heat,
                statusAmount = 1
            });
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1) };
    }
}