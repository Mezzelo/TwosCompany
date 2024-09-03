using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CurveTheBullet : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                mode = AStatusMode.Set,
                statusAmount = 1
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.droneShift,
                statusAmount = 1
            });
            actions.Add(new ADrawCard() {
                count = 2
            });
            if (upgrade != Upgrade.None)
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = upgrade == Upgrade.B ? Status.overdrive : Status.stunCharge,
                    statusAmount = 1
                });

            return actions;
        }

        public override string Name() => "Curve the Bullet";
    }
}
