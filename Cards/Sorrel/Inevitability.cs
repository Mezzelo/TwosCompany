using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Inevitability : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                buoyant = upgrade == Upgrade.A,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["Inevitability"].Id!,
                statusAmount = 1,
                dialogueSelector = ".mezz_carveReality",
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                    statusAmount = 2
                });
            return actions;
        }

        public override string Name() => "Inevitability";
    }
}
