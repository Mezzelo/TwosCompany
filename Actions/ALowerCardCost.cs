namespace TwosCompany.Actions {
    public class ALowerCardCost : CardAction {
        public int amount;
        public bool hand = false;
        public int selectedIndex = -1;
        public int minimum = -1;
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
                if (minimum >= 0)
                    selectedCard.discount = Math.Min(selectedCard.discount, Math.Max(selectedCard.discount + amount, -selectedCard.GetData(s).cost + minimum));
                else
                    selectedCard.discount += amount;
            } else {
                if (c.hand.Count == 0)
                    return;
                foreach (Card current in c.hand) {
                    if (minimum >= 0)
                        current.discount = Math.Min(current.discount, Math.Max(current.discount + amount, -current.GetData(s).cost + minimum));
                    else
                        current.discount += amount;
                }
            }
            Audio.Play(FSPRO.Event.Status_PowerUp);

        }
        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["LowerCostHint"]?.Head ??
                throw new Exception("missing glossary entry: LowerCostHint"), amount) };
    }
}