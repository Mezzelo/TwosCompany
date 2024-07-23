using TwosCompany.Cards.Jost;
using TwosCompany.Cards.Sorrel;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AReverseFrozen : CardAction {
        public override void Begin(G g, State s, Combat c) {
            Audio.Play(FSPRO.Event.Status_PowerUp);
            foreach (FrozenAttack fAttack in c.stuff.Values.Where(e => e is FrozenAttack)) {
                foreach (AAttack attack in fAttack.attacksHostile)
                    attack.targetPlayer = !attack.targetPlayer;
                foreach (AAttack attack in fAttack.attacks)
                    attack.targetPlayer = !attack.targetPlayer;
                List<AAttack> transfer = [.. fAttack.attacksHostile];
                fAttack.attacksHostile = fAttack.attacks;
                fAttack.attacks = transfer;
            }
        }
    }
}