namespace TwosCompany.Actions {
    public class ALowerCardCost : CardAction {
        public int amount;
        public bool hand = false;
        public int selectedIndex = -1;
        public override void Begin(G g, State s, Combat c) {
            if (!hand) {
                if (selectedIndex > -1) {
                    if (s.deck.Count <= selectedIndex)
                        return;
                    this.selectedCard = s.deck[selectedIndex];
                }
                Card selectedCard = this.selectedCard ?? throw new Exception("no card selected?");
                if (selectedCard == null)
                    return;
                selectedCard.discount -= amount;
            } else {
                if (c.hand.Count == 0)
                    return;
                foreach (Card current in c.hand)
                    current.discount -= amount;
            }
            Audio.Play(FSPRO.Event.Status_PowerUp);

        }
        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["LowerCostHint"]?.Head ??
                throw new Exception("missing glossary entry: LowerCostHint"), amount) };
    }
}