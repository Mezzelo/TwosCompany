using System.Collections.Generic;
using TwosCompany.Cards.Sorrel;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class ACollapseFrozen : CardAction {
        public int dir = -1;
        public int? last;
        public override void Begin(G g, State s, Combat c) {
            List<StuffBase> fAttacks = c.stuff.Values.OrderBy((StuffBase x) => (dir >= 0) ? (x.x) : -x.x).
                Where((StuffBase x) => x is FrozenAttack && (!last.HasValue ||
                last.HasValue && (dir >= 0 && x.x >= last || dir < 0 && x.x <= last))).ToList();
            for (int i = 0; i < fAttacks.Count; i++) {
                if (i < fAttacks.Count - 1 &&
                    Math.Abs(fAttacks[i].x - fAttacks[i + 1].x) == 1) {
                    FrozenAttack old = (FrozenAttack) fAttacks[i];
                    FrozenAttack to = (FrozenAttack) fAttacks[i + 1];
                    to.attacks.AddRange(old.attacks);
                    to.attacksHostile.AddRange(old.attacksHostile);
                    c.stuff.Remove(fAttacks[i].x);
                    fAttacks[i + 1].xLerped -= dir;
                    Audio.Play(FSPRO.Event.Move);
                    timer = 0.25;
                    c.QueueImmediate(new ACollapseFrozen() {
                        dir = this.dir,
                        last = fAttacks[i + 1].x,
                    });
                    return;
                }
            }
        }
    }
}