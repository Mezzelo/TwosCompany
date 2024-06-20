using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class SparkCard : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                temporary = true,
                recycle = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, 2),
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Spark";
    }
}
