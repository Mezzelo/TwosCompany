namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class InciendiaryRounds : Artifact {
        public bool firstHit = true;
        public override string Description() => "The first time your enemy is hit each turn, they gain your current <c=keyword>HEAT</c> + 1. " +
            "If they would <c=downside>overheat</c>, they do so immediately.";
        public override void OnEnemyGetHit(State state, Combat combat, Part? part) {
            if (firstHit) {
                firstHit = false;
                if (state.ship.Get(Status.heat) > -1) {
                    this.Pulse();
                    combat.QueueImmediate(new AStatus() {
                        targetPlayer = false,
                        status = Status.heat,
                        statusAmount = state.ship.Get(Status.heat) + 1,
                        dialogueSelector = state.ship.Get(Status.heat) > 2 ? ".mezz_inciendiaryRounds" : null,
                    });
                    if (combat.otherShip.Get(Status.heat) + state.ship.Get(Status.heat) + 1 >= combat.otherShip.heatTrigger) {
                        combat.Queue(new AOverheat() {
                            targetPlayer = false
                        });
                    }
                }
            }
        }

        public override void OnTurnEnd(State state, Combat combat) => firstHit = true;
        public override void OnCombatEnd(State state) => firstHit = true;
        // public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.heat", 3) };
    }
}