namespace TwosCompany.Actions {
    public class ADiscardSpecific : CardAction {
        public bool drawNotDiscard = false;
        public int discount = 0;
        public override void Begin(G g, State s, Combat c) {
            if (selectedCard == null)
                return;
            // don't do this if exhausted
            if (s.route is Combat) {
                bool found = false;
                foreach (Card card in c.exhausted) {
                    if (selectedCard.uuid == card.uuid)
                        found = true;
                }
                if (found)
                    return;
            }
            if (drawNotDiscard)
                Audio.Play(FSPRO.Event.CardHandling);
            else
                Audio.Play(FSPRO.Event.ZeroEnergy);
            if (discount != 0)
                selectedCard.discount += discount;
            s.RemoveCardFromWhereverItIs(selectedCard.uuid);
            selectedCard.OnDiscard(s, c);
            if (drawNotDiscard)
                s.SendCardToDeck(selectedCard);
            else
                c.SendCardToDiscard(s, selectedCard);
        }
    }
}