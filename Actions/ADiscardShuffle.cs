namespace TwosCompany.Actions {
    public class ADiscardShuffle : CardAction {
        public override void Begin(G g, State s, Combat c) {
            foreach (Card card in c.discard)
                s.SendCardToDeck(card, true, true);
            c.discard.Clear();
            s.ShuffleDeck(true);
        }
    }
}