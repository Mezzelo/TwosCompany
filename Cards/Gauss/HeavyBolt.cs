using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HeavyBolt : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade == Upgrade.B)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 2),
                });
            actions.Add(new AMove() {
                dir = 1,
                isRandom = true,
                targetPlayer = true,
            });
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 3),
                stunEnemy = upgrade == Upgrade.A,
            });
            return actions;
        }

        public override string Name() => "Heavy Bolt";
    }
}
