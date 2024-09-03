using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FalseOpening : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.overdrive,
                    statusAmount = 1,
                    mode = AStatusMode.Add,
                    targetPlayer = true,
                });
            else if (upgrade == Upgrade.B) {
                actions.Add(new AStatus() {
                    status = Status.tempPayback,
                    statusAmount = 1,
                    mode = AStatusMode.Add,
                    targetPlayer = true,
                });
                actions.Add(new AStatus() {
                    status = Status.stunCharge,
                    statusAmount = 1,
                    mode = AStatusMode.Add,
                    targetPlayer = true,
                });
            }
            actions.Add(new AForceAttack() {
                dialogueSelector = ".mezz_falseOpening",
            });
            return actions;
        }

        public override string Name() => "False Opening";
    }
}
