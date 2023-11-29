using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AlphaCore : Artifact {
        public override string Description() => "Gain 1 extra <c=energy>ENERGY</c> every turn.\r\n" +
            "At the start of every combat, <c=downside>exhaust 4 random cards in your draw pile.<c=downside>.";
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
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("cardtrait.exhaust") };
    }
}