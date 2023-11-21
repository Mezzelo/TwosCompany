// based largely on johanna's hook code, which i owe to EWanderer (along with the modloader obviously)

namespace TwosCompany.Actions {
    public class APDMove : CardAction {
        public bool isRight;

        public override void Begin(G g, State s, Combat c) {
            var move = CalculateMove(s, c, out var start_x);
            if (move == null) {
                return;
            }
            c.QueueImmediate(move);
        }
        public AMove? CalculateMove(State? s, Combat? c, out int start_x) {
            start_x = 0;
            if (s == null || c == null)
                return null;

            List<Part> cannons = (List<Part>) s.ship.parts.Where(e => e.type == PType.cannon && e.active);
            if (cannons.Count() == 0)
                return null;

            int start = s.ship.x + (isRight ? s.ship.parts.Count - 1 : 0);
            bool found = false;
            for (int i = start; isRight ? i < s.ship.parts.Count : i > 0; i += isRight ? 1 : -1)
                if (c.stuff.ContainsKey(i)) {
                    if (!(c.stuff[i] is Asteroid) && !(c.stuff[i] is Asteroid)) {
                        found = true;
                        break;
                    }
                }


            var move_action = new AMove() {
                dir = 1,
                fromEvade = false,
                ignoreHermes = true,
                ignoreFlipped = true,
                targetPlayer = true
            };
            return move_action;
        }

        public override Icon? GetIcon(State s) {
            return new Icon((Spr)(Manifest.Sprites[isRight ? "IconPDRight" : "IconPDLeft"].Id ?? 
                throw new Exception("missing icon")), 1, Colors.action);
        }

        public override List<Tooltip> GetTooltips(State s) {
            var list = new List<Tooltip>();
            AMove? move;
            var glossary = new TTGlossary(Manifest.Glossary["PersonalDefense"]?.Head ?? throw new Exception("missing glossary entry: PD"), 
                isRight? "rightmost" : "leftmost");
            list.Add(glossary);
            if (s.route is Combat c && (move = CalculateMove(s, c, out _)) != null) {
                var dir_key = isRight;
                list.AddRange(move.GetTooltips(s));
            }
            return list;
        }
    }
}