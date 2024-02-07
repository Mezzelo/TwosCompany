using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AConductorField : CardAction {
        public override void Begin(G g, State s, Combat c) {
            foreach (StuffBase stuffBase in Enumerable.ToList<StuffBase>((IEnumerable<StuffBase>)c.stuff.Values)) {
                c.stuff.Remove(stuffBase.x);
                Conduit cond = new Conduit() {
                    x = stuffBase.x,
                    xLerped = stuffBase.xLerped,
                    bubbleShield = stuffBase.bubbleShield,
                    age = stuffBase.age,

                };
                Conduit cond2 = cond;
                c.stuff[stuffBase.x] = cond2;
            }
            Audio.Play(FSPRO.Event.Status_PowerDown);
        }

        public override List<Tooltip> GetTooltips(State s) {
            if (s.route is Combat route) {
                foreach (StuffBase stuffBase in route.stuff.Values)
                    stuffBase.hilight = 2;
            }
            List<Tooltip> tooltips = new List<Tooltip>() {
                new TTGlossary(Manifest.Glossary["ConductorField"]?.Head ??
                throw new Exception("missing glossary entry: ConductorField")),
            };
            return tooltips;
        }

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconConductorField"].Id ?? throw new Exception("missing icon")), null, Colors.textMain);
    }
}
