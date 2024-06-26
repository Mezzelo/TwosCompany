using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class PressureReservoir : Artifact {
        public override string Description() => "Whenever you start your turn with 2+ <c=status>HEAT</c>, " +
            "convert 1 stack into <c=status>HEAT FEEDBACK</c>.";

        public override void OnTurnStart(State state, Combat combat) {
            if (state.ship.Get(Status.heat) > 1) {
                combat.Queue(new AStatus() {
                    status = Status.heat,
                    mode = AStatusMode.Add,
                    statusAmount = -1,
                    targetPlayer = true,
                    timer = 0.0,
                });
                combat.Queue(new AStatus() {
                    status = (Status) Manifest.Statuses?["HeatFeedback"].Id!,
                    mode = AStatusMode.Add,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = this.Key(),
                });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTGlossary("status.heat", 1),
            new TTGlossary("status." + Manifest.Statuses?["HeatFeedback"].Id, 1)
        };
    }
}