// based a good amount on johanna's hook code, which i owe to EWanderer (along with the modloader obviously)

namespace TwosCompany.Actions {
    public class APDMove : CardAction {
        public bool isRight;

        public override void Begin(G g, State s, Combat c) {
            var move = CalculateMove(s, c, out _);
            if (move == null)
                c.Queue(new ADiscardSpecific() {
                    selectedCard = this.selectedCard,
                    drawNotDiscard = false,
                    discount = 0
                });
            else
                c.QueueImmediate(move);
        }
        public AMove? CalculateMove(State? s, Combat? c, out int offset) {
            offset = 0;
            int move = 0;
            if (s == null || !(s.route is Combat) || c == null)
                return null;

            var cannons = s.ship.parts.Where(e => e.type == PType.cannon && e.active);
            if (cannons.Count() == 0)
                return null;

            int start = isRight ? 0 : s.ship.parts.Count - 1;
            bool found = false;
            for (int i = start; (isRight ? i < s.ship.parts.Count : i >= 0) && !found; i += isRight ? 1 : -1)
                if (c.stuff.ContainsKey(s.ship.x + i))
                    if (!(c.stuff[s.ship.x + i] is Asteroid) && !(c.stuff[s.ship.x + i] is Asteroid) &&
                        c.stuff[s.ship.x + i].IsHostile()) 
                        for (int g = i; isRight ? g >= 0 : g < s.ship.parts.Count; g += isRight ? -1 : 1)
                            if (s.ship.parts[g].type == PType.cannon) {
                                move = i - g;
                                offset = i;
                                found = true;
                                break;
                            }

            if (!found)
                return null;

            var move_action = new AMove() {
                dir = move,
                ignoreHermes = true,
                ignoreFlipped = true,
                targetPlayer = true
            };
            return move_action;
        }

        public override Icon? GetIcon(State s) {
            return new Icon((Spr)(Manifest.Sprites[isRight ? "IconPointDefense" : "IconPointDefenseLeft"].Id ?? 
                throw new Exception("missing icon")), null, Colors.action);
        }

        public override List<Tooltip> GetTooltips(State s) {
            var list = new List<Tooltip>() {
                new TTGlossary(Manifest.Glossary["PointDefense"]?.Head ?? throw new Exception("missing glossary entry: PD"),
                isRight ? "right" : "left", isRight ? "leftmost" : "rightmost")
            };
            AMove? move;
            int offset;
            if (s.route is Combat c && (move = CalculateMove(s, c, out offset)) != null) {
                list.AddRange(move.GetTooltips(s));
                if (c.stuff.ContainsKey(s.ship.x + offset)) {
                    c.stuff[s.ship.x + offset].hilight = 2;
                }
            }
            return list;
        }
    }
}