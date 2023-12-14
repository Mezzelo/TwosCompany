namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class CannonGuard : Artifact {

        public int pos = 0;
        public bool markForGain = false;
        public override string Description() => "Whenever you end your turn in the same position you started, gain 1 <c=status>OVERDRIVE</c>.";
        public override void OnTurnStart(State state, Combat combat) {
            if (markForGain) {
                this.Pulse();
                markForGain = false;
                combat.Queue(new AStatus() {
                    targetPlayer = true,
                    status = Status.overdrive,
                    statusAmount = 1,
                    dialogueSelector = ".mezz_cannonGuard"
                });
            }
            pos = state.ship.x;
        }
        public override void OnTurnEnd(State state, Combat combat) => markForGain = state.ship.x == pos;

        public override void OnCombatStart(State state, Combat combat) {
            pos = state.ship.x;
        }
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1) };
    }
}