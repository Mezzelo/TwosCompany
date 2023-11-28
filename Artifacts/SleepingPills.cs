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

                combat.Queue(new AStatus() {
                    targetPlayer = true,
                    status = Status.serenity,
                    statusAmount = 1,
                    timer = 0.5
                });

                if (state.ship.hull >= state.ship.hullMax)
                    return;
                int num = 1;
                foreach (Artifact enumerateAllArtifact in state.EnumerateAllArtifacts())
                    num += enumerateAllArtifact.ModifyHealAmount(1, state);
                combat.Queue(new AHeal() {
                    healAmount = num,
                    targetPlayer = true,
                    timer = 0.5
                });
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