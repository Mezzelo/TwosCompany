using TwosCompany.Cards.Sorrel;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class ADoubleFrozen : CardAction {

        public int mult = 2;
        public override void Begin(G g, State s, Combat c) {
            Audio.Play(FSPRO.Event.Status_PowerUp);
            foreach (FrozenAttack fAttack in c.stuff.Values.Where(e => e is FrozenAttack)) {
                foreach (AAttack attack in fAttack.attacks)
                    attack.damage *= mult;
                foreach (AAttack attack in fAttack.attacksHostile)
                    attack.damage *= mult;
            }
        }

    }
}