using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class MoveAsOne : Card, IJostCard, IOtherAttackIncreaseCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 3 : 4,
                retain = upgrade == Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSpriteDown1" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public int costIncrease = 0;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ACostDecreaseAttackHint() {
                amount = 2,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.A ? 4 : 3,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new ADummyTooltip() {
                action = Stance.Get(s) % 2 != 1 ? new ACostDecreaseAttackHint() : null
            });

            actions.Add(new AAttack() {
                damage = GetDmg(s, 4),
                stunEnemy = true,
                piercing = upgrade == Upgrade.A,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease = 0;
        }
        public override void OnExitCombat(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }

        public void OtherAttackDiscount(State s) {
            if (Stance.Get(s) % 2 == 1) {
                costIncrease += 2;
                this.discount -= 2;
            }
        }

        public override string Name() => "Move as One";
    }
}
