using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class DamageControl : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 2 : 3,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus uncannyStatus = Manifest.Statuses?["UncannyEvasion"] ?? throw new Exception("status missing: uncannyevasion");
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = uncannyStatus.Id != null ? (Status)uncannyStatus.Id : Status.drawNextTurn,
                statusAmount = 1
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = Status.autododgeRight,
                    statusAmount = 1
                });
            return actions;
        }

        public override string Name() => "Damage Control";
    }
}
