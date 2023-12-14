using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Ignition : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.powerdrive,
                    statusAmount = 1,
                    targetPlayer = true
                });

            ExternalStatus enflamedStatus = Manifest.Statuses?["Enflamed"] ?? throw new Exception("status missing: enflamed");
            actions.Add(new AStatus() {
                status = enflamedStatus.Id != null ? (Status)enflamedStatus.Id : Status.heat,
                statusAmount = 1,
                targetPlayer = true
            });
            actions.Add(new AHeal() {
                targetPlayer = true,
                healAmount = 1,
                canRunAfterKill = true
            });

            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true
                });
            return actions;
        }

        public override string Name() => "Ignition";
    }
}
