namespace TwosCompany.Actions {
    public class ACaptainsOrders : CardAction {
        public bool dontExhaust = false;
        public override void Begin(G g, State s, Combat c) {
            Card selectedCard = this.selectedCard ?? throw new Exception("no card selected?");
            if (selectedCard == null)
                return;
            c.TryPlayCard(s, selectedCard, true);
            CardData data = selectedCard.GetDataWithOverrides(s);
            c.energy += data.cost;
            if (data.cost > 0) {
                c.pulseEnergyGood = 0.5;
            }
            if (data.cost != 0)
                Audio.Play(FSPRO.Event.Status_PowerUp);
            if (dontExhaust && data.exhaust) {
                s.RemoveCardFromWhereverItIs(selectedCard.uuid);
                c.SendCardToDiscard(s, selectedCard);
            }

        }
        public override Icon? GetIcon(State s) => new Icon?(new Icon(Spr.icons_bypass, 0, Colors.textMain));
        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = new List<Tooltip> { new TTGlossary("action.bypass") };
            if (dontExhaust)
                list.Add(new TTGlossary("cardtrait.exhaust"));
            return list;
        }
    }
}