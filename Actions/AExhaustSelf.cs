using FSPRO;

namespace TwosCompany.Actions {
    public class AExhaustSelf : CardAction {
        public override void Begin(G g, State s, Combat c) {

        }
        public override Icon? GetIcon(State s) => new Icon?(new Icon(Enum.Parse<Spr>("icons_exhaust"), new int?(), Colors.textMain));
        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary("cardtrait.exhaust") };
    }
}
