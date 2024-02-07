using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Practiced : Card, IJostCard {

        public int costIncrease = 0;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
                infinite = upgrade == Upgrade.B && Stance.Get(state) > 0,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAllIncreaseHint() {
                amount = 1,
                isCombat = upgrade != Upgrade.B,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = costIncrease + (upgrade == Upgrade.A ? 3 : (upgrade == Upgrade.B ? 1 : 2)),
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());

            if (upgrade == Upgrade.B)
                actions.Add(new AAllIncreaseHint() {
                    amount = 1,
                    isCombat = upgrade != Upgrade.B,
                    disabled = Stance.Get(s) < 2,
                });
            else
                actions.Add(new AAttack() {
                    damage = GetDmg(s, costIncrease + (upgrade == Upgrade.A ? 2 : 1) + (Stance.Get(s) == 3 ? 1 : 0)),
                    fast = upgrade != Upgrade.B,
                    disabled = Stance.Get(s) < 2,
                });
            actions.Add(new AAttack() {
                damage = GetDmg(s, costIncrease + (upgrade == Upgrade.B ? 1 : 2) + (Stance.Get(s) == 3 ? 1 : 0)),
                fast = upgrade != Upgrade.B,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }
        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 0;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade == Upgrade.B) {
                this.discount -= costIncrease;
                costIncrease = 0;
            }

        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (Stance.Get(state) % 2 == 1 || (upgrade == Upgrade.B && Stance.Get(state) > 0))
                costIncrease += upgrade == Upgrade.B && Stance.Get(state) == 3 ? 2 : 1;
            
            this.discount += costIncrease;
        }

        public override string Name() => "Practiced";
    }
}
