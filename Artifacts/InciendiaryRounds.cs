namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class InciendiaryRounds : Artifact {
        public bool firstHit = true;
        public override void OnEnemyGetHit(State state, Combat combat) {
            if (firstHit) {
                firstHit = false;
                if (state.ship.Get(Status.heat) > 0) {
                    this.Pulse();
                    combat.QueueImmediate(new AStatus() {
                        targetPlayer = false,
                        status = Status.heat,
                        statusAmount = state.ship.Get(Status.heat)
                    });
                }
            }
        }

        public override void OnTurnEnd(State state, Combat combat) => firstHit = true;
        public override void OnCombatEnd(State state) => firstHit = true;
    }
}