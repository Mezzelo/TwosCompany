using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HoldOn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.hermes,
                statusAmount = upgrade == Upgrade.A ? 2 : 3,
                dialogueSelector = ".mezz_holdOn"
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus()
                {
                    targetPlayer = true,
                    status = Status.evade,
                    statusAmount = 1
                });
            else if (upgrade == Upgrade.B) {
                ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = strafeStatus.Id != null ? (Status)strafeStatus.Id : Status.strafe,
                    statusAmount = 1
                });
            }
            return actions;
        }

        public override string Name() => "Hold On!";
    }
}
