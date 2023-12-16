namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class JerryCan : Artifact {
        public override string Description() => "At the start of combat, <c=healing>heal 1</c> and <c=downside>gain 2 HEAT</c>.";
        public override void OnCombatStart(State state, Combat combat) {
            this.Pulse();
            combat.QueueImmediate(new AStatus() {
            targetPlayer = true,
                status = Status.heat,
                statusAmount = 2
            });

            if (state.ship.hull >= state.ship.hullMax)
                return;
            int num = 1;
            foreach (Artifact enumerateAllArtifact in state.EnumerateAllArtifacts())
                num += enumerateAllArtifact.ModifyHealAmount(1, state, true);
            combat.Queue(new AHeal() {
                healAmount = num,
                targetPlayer = true,
                timer = 0.5
            });
        }
        // public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.heat", 3) };

    }
}