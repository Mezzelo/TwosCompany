namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AlphaCore : Artifact {
        public override void OnCombatStart(State state, Combat combat) {
            this.Pulse();
            for (int i = 0; i < 4; i++) {
                if (state.deck.Count <= 0)
                    break;
                Card thisCard = state.deck[state.rngActions.NextInt() % state.deck.Count];
                state.RemoveCardFromWhereverItIs(thisCard.uuid);
                combat.SendCardToExhaust(state, thisCard);
            }
        }

        public override void OnReceiveArtifact(State state) => ++state.ship.baseEnergy;
        public override void OnRemoveArtifact(State state) => --state.ship.baseEnergy;
    }
}