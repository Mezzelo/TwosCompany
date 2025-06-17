using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Finesse : Card, IOtherAttackIncreaseCard, ITCNickelTraits {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = upgrade == Upgrade.A,
            };
        }

        public int costIncrease = 0;
        public string[] GetTraits()
            => new string[] { "EnergyPerAttack" };
        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (!Manifest.hasNickel)
                actions.Add(new ACostDecreaseAttackHint() {
                    amount = 1,
                });
            if (upgrade != Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                });
            actions.Add(new AMove() {
                dir = 1,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    stunEnemy = true
                });
            return actions;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease = 0;
        }
        public override void OnExitCombat(State s, Combat c) {
            this.discount += costIncrease;
            costIncrease = 0;
        }
        public void OtherAttackDiscount(State s) {
            costIncrease++;
            this.discount--;
        }

        public override string Name() => "Finesse";
    }
}
