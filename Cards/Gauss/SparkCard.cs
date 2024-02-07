using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SparkCard : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                recycle = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
            });
            return actions;
        }

        public override string Name() => "Spark";
    }
}
