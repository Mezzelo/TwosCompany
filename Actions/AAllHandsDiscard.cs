namespace TwosCompany.Actions {
    public class AllHandsDiscard : CardAction {
        public override void Begin(G g, State s, Combat c) {
            if (selectedCard == null)
                return;
            Audio.Play(FSPRO.Event.ZeroEnergy);
            s.RemoveCardFromWhereverItIs(selectedCard.uuid);
            c.SendCardToDiscard(s, selectedCard);
        }
    }
}