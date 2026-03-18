using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Encore : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                exhaust = upgrade != Upgrade.B,
                buoyant = upgrade == Upgrade.A,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status) Manifest.Statuses?["Encore"].Id!,
                statusAmount =1,
            });
            return actions;
        }

        public override string Name() => "Encore";
    }
}
