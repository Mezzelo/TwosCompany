using TwosCompany;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class ShieldShunt : Artifact {
        public int counter = 0;
        public bool lastWasMove = false;

        public override void OnReceiveArtifact(State state) {
            int increase = state.ship.shieldMaxBase *= 2;
            state.ship.shieldMaxBase = 1;
            state.ship.hullMax += increase;
            int num = increase;
            foreach (Artifact enumerateAllArtifact in state.EnumerateAllArtifacts())
                num += enumerateAllArtifact.ModifyHealAmount(increase, state);
            state.ship.hull += num;
            if (state.ship.hull <= state.ship.hullMax)
                return;
            state.ship.hull = state.ship.hullMax;
        }

        public override void OnRemoveArtifact(State state) {
        }
    }
}