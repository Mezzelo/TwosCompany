namespace TwosCompany.Actions {
    public class StatCostAction : CardAction {
        public Status statusReq;
        public int statusCost;
        public CardAction? action;
        public override void Begin(G g, State s, Combat c) {
            if (action == null)
                return;
            if (s.ship.statusEffects.ContainsKey(statusReq) && s.ship.statusEffects[statusReq] > statusCost) {
                c.QueueImmediate(new AStatus() {
                    targetPlayer = true,
                });
                c.QueueImmediate(action);
            }
        }
    }
}
