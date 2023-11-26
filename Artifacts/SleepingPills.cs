using TwosCompany;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class SleepingPills : Artifact {
        public int counter = 0;
        // public override void OnCombatEnd(State state) => counter = 0;
        public override int? GetDisplayNumber(State s) => counter == 0 ? null : counter;

        public override void AfterPlayerOverheat(State state, Combat combat) {
            if (counter <= 0) {
                counter = 4;
                this.Pulse();

                combat.QueueImmediate(new AStatus() {
                    targetPlayer = true,
                    status = Status.serenity,
                    statusAmount = 1
                });

                int baseAmount = 1;
                int num = baseAmount;
                foreach (Artifact enumerateAllArtifact in state.EnumerateAllArtifacts())
                    num += enumerateAllArtifact.ModifyHealAmount(baseAmount, state);
                state.ship.hull += num;
                if (state.ship.hull <= state.ship.hullMax)
                    return;
                state.ship.hull = state.ship.hullMax;
            }
        }
        public override void OnTurnEnd(State state, Combat combat) {
            if (counter > 0)
                counter--;
        }

        public override void OnRemoveArtifact(State state) {
        }
    }
}