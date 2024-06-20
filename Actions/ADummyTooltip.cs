namespace TwosCompany.Actions {
    public class ADummyTooltip : ADummyAction {
        public CardAction? action;
        public Card? tooltipCard;
        public override List<Tooltip> GetTooltips(State s) {
            if (action != null)
                return action.GetTooltips(s);
            if (tooltipCard != null)
                return new List<Tooltip>() { new TTCard() { card = tooltipCard } };
            return new List<Tooltip>();
        }
    }
}