using Microsoft.Extensions.Logging;

namespace TwosCompany.Actions {
    public class AAsteroidBelt : CardAction {
        public bool bubbled = false;
        public override void Begin(G g, State s, Combat c) {
            bool hasSpawned = false;
            for (int i = -1; i < s.ship.parts.Count + 1; i++) {
                if (!c.stuff.ContainsKey(s.ship.x + i) && ((c.stuff.ContainsKey(s.ship.x + i - 1) && i > 0) ||
                        (c.stuff.ContainsKey(s.ship.x + i + 1) && i < s.ship.parts.Count - 1))) {
                    c.Queue(new ASpawn() {
                        fromX = i,
                        fromPlayer = true,
                        thing = new Asteroid() {
                            yAnimation = 0.0,
                            bubbleShield = bubbled,
                        },
                        timer = 0.1,
                        multiBayVolley = true,
                        dialogueSelector = hasSpawned ? null : ".mezz_asteroidField",
                    });
                    hasSpawned = true;
                }
            }
        }

        public override List<Tooltip> GetTooltips(State s) {
            var list = new List<Tooltip>() {
                new TTGlossary("action.spawn"),
            };
            list.AddRange(new Asteroid() { bubbleShield = bubbled }.GetTooltips());

            if (s.route is Combat c) {
                for (int i = 0; i < s.ship.parts.Count; i++) {
                    if (c.stuff.ContainsKey(s.ship.x + i) && (!c.stuff.ContainsKey(s.ship.x + i - 1) || !c.stuff.ContainsKey(s.ship.x + i + 1))) {
                        c.stuff[s.ship.x + i].hilight = 2;
                    }
                }
            }

            return list;
        }
    }
}