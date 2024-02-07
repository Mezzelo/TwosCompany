using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Electrocute : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = Status.stunCharge,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
            });
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, upgrade == Upgrade.A ? 1 : 0),
            });
            return actions;
        }

        public override string Name() => "Electrocute";
    }
}
