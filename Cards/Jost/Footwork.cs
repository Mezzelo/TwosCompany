using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Footwork : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?["Footwork"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 2,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.drawNextTurn,
                    statusAmount = 2,
                    targetPlayer = true,
                });

            return actions;
        }

        public override string Name() => "Footwork";
    }
}
