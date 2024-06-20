using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FalseOpening : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = 2,
                mode = AStatusMode.Add,
                targetPlayer = true,
            });
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
                dialogueSelector = ".mezz_falseOpening",
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AEndTurn());
            return actions;
        }

        public override string Name() => "False Opening";
    }
}
