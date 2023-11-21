namespace TwosCompany.Actions {
    public class ALowerCardCost : CardAction {
        public int amount;
        public bool hand = false;
        public int selectedIndex = -1;
        public override void Begin(G g, State s, Combat c) {
            if (!hand) {
                if (selectedIndex > -1) {
                    if (c.hand.Count <= selectedIndex)
                        return;
                    this.selectedCard = c.hand[selectedIndex];
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
        public override List<Tooltip> GetTooltips(State s) {
            var list = new List<Tooltip>();
            var glossary = new TTGlossary(Manifest.Glossary["LowerCostHint"]?.Head ?? throw new Exception("missing glossary entry: LowerCostHint"), amount);
            list.Add(glossary);
            return list;
        }
    }
}