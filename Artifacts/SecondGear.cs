namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class SecondGear : Artifact {
        public int counter = 0;
        // public override void OnCombatEnd(State state) => counter = 0;
        public override int? GetDisplayNumber(State s) => counter > 5 || counter < 1 ? null : counter;

        public override void OnTurnStart(State state, Combat combat) {
            counter++;
            if (counter == 6) {
                this.Pulse();
                Ship ship = state.ship;
                foreach (KeyValuePair<Status, int> thisStatus in ship.statusEffects) {
                    if (thisStatus.Value == 0)
                        continue;
                    if (
                        thisStatus.Key != Status.shield
                        && thisStatus.Key != Status.tempShield
                        // && thisStatus.Key != Status.maxShield
                        && thisStatus.Key != Status.shard
                    // && thisStatus.Key != Status.evade
                    // && thisStatus.Key != Status.maxShard
                        )
                        combat.Queue(new AStatus() {
                            targetPlayer = true,
                            status = thisStatus.Key,
                            statusAmount = thisStatus.Value
                        });
                }
            }
        }

        public override void OnCombatEnd(State state) => counter = 0;
    }
}