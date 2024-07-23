using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LikeTears : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["FrozenStun"].Id!,
                statusAmount = upgrade == Upgrade.A ? 2 : 1
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = 2
                });
            return actions;
        }

        public override string Name() => "Azoth";
    }
}
