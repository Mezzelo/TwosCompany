using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class DistantStrike : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses["DistantStrike"].Id!,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.stunCharge,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Distant Strike";
    }
}
