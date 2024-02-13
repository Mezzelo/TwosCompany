namespace TwosCompany.Actions {
    public class ADummyTooltip : ADummyAction {
        public CardAction? action;
        public override List<Tooltip> GetTooltips(State s) => (action != null) ? action.GetTooltips(s) : new List<Tooltip>();
    }
}