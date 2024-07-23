using TwosCompany.Cards.Sorrel;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AUnfreeze : CardAction {

        public bool omitIncoming = false;

        public override void Begin(G g, State s, Combat c) {
            foreach (int item in c.stuff.Keys.OrderBy((int n) => n)) {
                if (c.stuff[item] is FrozenAttack fAttack) {
                    fAttack.unfreeze = true;
                    c.Queue(fAttack.GetActions(s, c));
                }
            }
        }

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconUnfreeze"].Id ??
            throw new Exception("missing icon")), null, Colors.status);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["Unfreeze"]?.Head ??
            throw new Exception("missing glossary entry: Unfreeze")) };
    }
}