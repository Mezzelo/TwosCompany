namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class JerryCan : Artifact {
        public override string Description() => "At the start of combat, <c=healing>heal 1</c> and <c=downside>gain 2 HEAT</c>.";
        public override void OnCombatStart(State state, Combat combat) {
            combat.QueueImmediate(new AStatus() {
            targetPlayer = true,
                status = Status.heat,
                statusAmount = 2,
                artifactPulse = this.Key(),
            });

            if (state.ship.hull >= state.ship.hullMax)
                return;
            combat.Queue(new AHeal() {
                healAmount = 1,
                targetPlayer = true,
                timer = 0.5
            });
        }
        // public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.heat", 3) };

    }
}