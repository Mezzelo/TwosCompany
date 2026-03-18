using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SparkCard : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                recycle = upgrade == Upgrade.A,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, upgrade == Upgrade.B ? 1 : 2),
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 0),
                });
            return actions;
        }

        public override string Name() => "Spark";
    }
}
