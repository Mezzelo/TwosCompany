using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EveryFrameAPainting : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                buoyant = upgrade == Upgrade.A,
                retain = true,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.timeStop,
                statusAmount = 1
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = 1,
                dialogueSelector = ".mezz_postpone",
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = Status.boost,
                    statusAmount = 1
                });

            return actions;
        }

        public override string Name() => "Every Frame a Painting";
    }
}
