using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Relentless : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 3 : 1,
                retain = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.libra,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                dialogueSelector = c.energy >= 2 ? ".mezz_relentless" : null,
            });
            ExternalStatus mobileStatus = Manifest.Statuses?["MobileDefense"] ?? throw new Exception("status missing: mobile defense");
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = mobileStatus.Id != null ? (Status)mobileStatus.Id : Status.tempShield,
                statusAmount = upgrade == Upgrade.B ? 2 : 1
            });
            return actions;
        }

        public override string Name() => "Relentless";
    }
}
