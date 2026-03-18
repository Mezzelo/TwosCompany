using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Patience : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = true,
                infinite = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.drawNextTurn,
                statusAmount = upgrade == Upgrade.A ? 3 : 1,
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.energyNextTurn,
                statusAmount = 1,
            });
            return actions;
        }

        public override string Name() => "Patience";
    }
}
