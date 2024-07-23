namespace TwosCompany.Actions {
    public class AIncendOverheat : CardAction {
        public override void Begin(G g, State s, Combat c) {
            if (s.route is not Combat)
                return;
            if (c.otherShip.Get(Status.heat) >= c.otherShip.heatTrigger) {
                c.QueueImmediate(new AOverheat() {
                    targetPlayer = false,
                    dialogueSelector = ".mezz_incendiaryRounds",
                    timer = 0.0,
                });
            }
        }
    }
}