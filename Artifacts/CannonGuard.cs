namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class CannonGuard : Artifact {

        public int pos = 0;
        public bool markForGain = false;
        public override void OnTurnStart(State state, Combat combat) {
            if (markForGain) {
                this.Pulse();
                markForGain = false;
                combat.Queue(new AStatus() {
                    targetPlayer = true,
                    status = Status.overdrive,
                    statusAmount = 1
                });
            }
            pos = state.ship.x;
        }
        public override void OnTurnEnd(State state, Combat combat) => markForGain = state.ship.x == pos;

        public override void OnCombatStart(State state, Combat combat) {
            pos = state.ship.x;
        }
    }
}