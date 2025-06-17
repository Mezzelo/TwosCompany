using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Bind : Card, IOtherAttackIncreaseCard, ITCNickelTraits {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = true,
            };
        }

        public int costIncrease = 0;
        public string[] GetTraits()
            => new string[] { "EnergyPerAttackIncrease" };

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (!Manifest.hasNickel)
                actions.Add(new ACostIncreaseAttackHint() {
                    amount = 1,
                });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.shield,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = upgrade == Upgrade.A ? 2 : 3,
                targetPlayer = true,
                dialogueSelector = ".mezz_bind"
            });
            actions.Add(new AStatus() {
                status = Status.engineStall,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B) {
                ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
                actions.Add(new AStatus() {
                    status = strafeStatus.Id != null ? (Status)strafeStatus.Id : Status.strafe,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            }
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
            costIncrease++;
            this.discount++;
        }
        public override void OnDiscard(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }

        public override string Name() => "Bind";
    }
}
