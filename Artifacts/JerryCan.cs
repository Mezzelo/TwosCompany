using TwosCompany;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class JerryCan : Artifact {
        public override void OnCombatStart(State state, Combat combat) {
            this.Pulse();
            combat.QueueImmediate(new AStatus() {
            targetPlayer = true,
                status = Status.heat,
                statusAmount = 2
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
}