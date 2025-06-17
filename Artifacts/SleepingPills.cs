using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class SleepingPills : Artifact {
        public int counter = 0;
        // public override void OnCombatEnd(State state) => counter = 0;
        public override string Description() => ManifArtifactHelper.artifactTexts["SleepingPills"];
        public override int? GetDisplayNumber(State s) => counter == 0 ? null : counter;

        public override void AfterPlayerOverheat(State state, Combat combat) {
            if (counter <= 0) {
                counter = 4;
                this.Pulse();

                combat.Queue(new AStatus() {
                    targetPlayer = true,
                    status = Status.serenity,
                    statusAmount = 1,
                    timer = 0.5,
                    dialogueSelector = ".mezz_sleepingPills",
                });

                if (state.ship.hull >= state.ship.hullMax)
                    return;
                combat.Queue(new AHeal() {
                    healAmount = 1,
                    targetPlayer = true,
                    timer = 0.5
                });
            }
        }
        public override void OnTurnEnd(State state, Combat combat) {
            if (counter > 0)
                counter--;
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.serenity", 1) };
    }
}