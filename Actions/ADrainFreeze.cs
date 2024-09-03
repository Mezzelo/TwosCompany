using TwosCompany.Cards.Sorrel;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class ADrainFreeze : CardAction {

        public FrozenAttack? attack;
        public bool noFx = true;

        public override void Begin(G g, State s, Combat c) {
            if (attack == null)
                return;
            if (attack.bubbleShield) {
                attack.bubbleShield = false;
                Audio.Play(FSPRO.Event.Hits_HitDrone);
            }
            if (attack.attacks.Count == 0 && attack.attacksHostile.Count == 0) {
                c.QueueImmediate(new AChainDestroyDrone() {
                    stuff = attack,
                    noFx = this.noFx,
                    fxOverride = false,
                    playerDidIt = false,
                    timer = 0.0,
                });
                return;
            }
            c.QueueImmediate(new ADrainFreeze() {
                attack = this.attack,
                timer = 0.0,
                noFx = this.noFx,
            });
            if (attack.attacks.Count > 0) {
                attack.attacks[0].fromDroneX = attack.x;
                attack.attacks[0].fast = true;
                attack.attacks[0].stunEnemy = attack.attacks[0].stunEnemy ||
                    s.ship.Get((Status) Manifest.Statuses["FrozenStun"].Id!) > 0;
                attack.attacks[0].cardOnHit = null;
                c.QueueImmediate(attack.attacks[0]);
                attack.attacks.RemoveAt(0);
            } else if (attack.attacksHostile.Count > 0) {
                attack.attacksHostile[0].fromDroneX = attack.x;
                attack.attacksHostile[0].fast = true;
                attack.attacksHostile[0].stunEnemy = false;
                c.QueueImmediate(attack.attacksHostile[0]);
                attack.attacksHostile.RemoveAt(0);
            }
        }
    }
}