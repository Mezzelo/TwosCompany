namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class ShieldShunt : Artifact {

        public override void OnReceiveArtifact(State state) {

            int increase = state.ship.shieldMaxBase *= 2;
            int num = increase;
            foreach (Artifact enumerateAllArtifact in state.EnumerateAllArtifacts())
                num += enumerateAllArtifact.ModifyHealAmount(increase, state);

            state.rewardsQueue.Add(new AHullMax() {
                amount = increase,
                targetPlayer = true,
                timer = 0.5
            });
            state.rewardsQueue.Add(new AHeal() {
                healAmount = num,
                targetPlayer = true,
                timer = 0.5
            });
            state.rewardsQueue.Add(new AShieldMax() {
                amount = -state.ship.shieldMaxBase + 1,
                targetPlayer = true,
                timer = 0.5
            });
        }

        public override void OnRemoveArtifact(State state) {
        }
    }
}