namespace TwosCompany.Actions {
    public class ADiscardSpecific : CardAction {
        public override void Begin(G g, State s, Combat c) {
            if (selectedCard == null)
                return;
            Audio.Play(FSPRO.Event.ZeroEnergy);
            s.RemoveCardFromWhereverItIs(selectedCard.uuid);
            selectedCard.OnDiscard(s, c);
            c.SendCardToDiscard(s, selectedCard);
        }
    }
}