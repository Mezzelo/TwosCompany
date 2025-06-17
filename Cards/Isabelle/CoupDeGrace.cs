using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CoupDeGrace : Card, IOtherAttackIncreaseCard, ITCNickelTraits {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 7 : 5,
                retain = upgrade == Upgrade.B,
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

            actions.Add(new AAttack() {
                damage = GetDmg(s, 6),
                stunEnemy = true,
                piercing = upgrade == Upgrade.A,
                dialogueSelector = ".mezz_coupDeGrace",
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

        public override string Name() => "Coup De Grace";

    }
}
