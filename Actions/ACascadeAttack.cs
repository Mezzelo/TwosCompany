using System.Collections.Generic;
using System;
using System.Collections;

namespace TwosCompany.Actions {
    public class ACascadeAttack : CardAction {

        public int dir = 1;
        public int? originalPos;

        public override void Begin(G g, State s, Combat c) {
            if (originalPos != null && originalPos == s.ship.x)
                return;
            // check for hit
            bool hit = false;
            for (int i = 0; i < s.ship.parts.Count; i++) {
                if (s.ship.parts[i].type == PType.cannon && s.ship.parts[i].active) {
                    RaycastResult ray = CombatUtils.RaycastFromShipLocal(s, c, i, false);
                    if (ray != null && ((ray.hitShip && 
                        c.otherShip.Get(Status.autododgeLeft) <= 0 && c.otherShip.Get(Status.autododgeRight) <= 0) ||
                        ray.hitDrone)) {
                        hit = true;
                        break;
                    }
                }
            }
            c.QueueImmediate(new AAttack() {
                damage = Card.GetActualDamage(s, 1),
                fast = true,
                targetPlayer = false,
            });
            if (hit) {
                c.Queue(new AMove() {
                    dir = this.dir,
                    targetPlayer = true,
                });
                c.Queue(new ACascadeAttack() {
                    dir = this.dir,
                    originalPos = s.ship.x,
                    timer = 0.0,
                });
            }
        }
    }
}
