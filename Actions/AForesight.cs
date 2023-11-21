namespace TwosCompany.Actions {
    public class AForesight : CardAction {
        public bool exhaust = false;
        public int deckIndex = -1;
        public override void Begin(G g, State s, Combat c) {
            if (this.selectedCard == null) {
                if (deckIndex < 0 || s.deck.Count <= deckIndex)
                    return;
                this.selectedCard = s.deck[deckIndex];
                
            }
            Card selectedCard = this.selectedCard ?? throw new Exception("no card selected?");
            Audio.Play(FSPRO.Event.CardHandling);
            s.RemoveCardFromWhereverItIs(selectedCard.uuid);
            if (exhaust)
                c.SendCardToExhaust(s, selectedCard);
            else
                c.SendCardToDiscard(s, selectedCard);
            if (deckIndex > -1)
                c.QueueImmediate(new ADelay() {
                    time = 0.3
                });
        }
        public override List<Tooltip> GetTooltips(State s) {
            if (exhaust)
                return new List<Tooltip>() { new TTGlossary("cardtrait.exhaust") };
            else
                return new List<Tooltip>();
        }
    }
}