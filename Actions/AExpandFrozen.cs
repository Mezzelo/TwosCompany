using TwosCompany.Cards.Sorrel;

namespace TwosCompany.Actions {
    public class AExpandFrozen : CardAction {
        public int dir = 1;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;
    }
}