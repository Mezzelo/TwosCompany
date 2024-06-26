using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class StaticCharge : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 4 : 3,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses["ElectrocuteCharge"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 0),
                    stunEnemy = true,
                });
            return actions;
        }

        public override string Name() => "Static Charge";
    }
}
