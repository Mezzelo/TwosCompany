using System.Collections.Generic;
using TwosCompany.Cards.Sorrel;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AShatterFrozen : CardAction {
        public bool weaken = false;
        public bool armorize = false;
        public bool onlyOutgoing = false;

        public override void Begin(G g, State s, Combat c) {
            Audio.Play(FSPRO.Event.Status_PowerUp);
            List<StuffBase> fAttacks = c.stuff.Values.Where((StuffBase x) => x is FrozenAttack).ToList();
            foreach (FrozenAttack fAttack in fAttacks) {
                foreach (AAttack attack in fAttack.attacks) {
                    if (weaken)
                        attack.weaken = true;
                    else if (armorize)
                        attack.armorize = true;
                }
                if (!onlyOutgoing) {
                    foreach (AAttack attack in fAttack.attacksHostile) {
                        if (weaken)
                            attack.weaken = true;
                        else if (armorize)
                            attack.armorize = true;
                    }
                }
            }
        }

        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> tooltips = new List<Tooltip>();
            if (weaken)
                tooltips.Add(new TTGlossary("parttrait.weak"));
            else if (armorize)
                tooltips.Add(new TTGlossary("parttrait.armor"));
            return tooltips;
        }
    }
}