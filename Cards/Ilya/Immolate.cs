using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Immolate : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 3,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new StatCostAction() {
                    action = new AStatus() {
                        status = Status.shield,
                        targetPlayer = true,
                        statusAmount = 2,
                    },
                    statusReq = Status.heat,
                    statusCost = 1,
                    cumulative = 0,
                    first = true,
                });
            actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    damage = GetDmg(s, upgrade == Upgrade.A ? 1 : 4),
                    fast = upgrade == Upgrade.A,
                },
                statusReq = Status.heat,
                statusCost = upgrade == Upgrade.A ? 1 : 3,
                cumulative = upgrade == Upgrade.B ? 1 : 0,
                first = upgrade != Upgrade.B
            });
            if (upgrade == Upgrade.A)
                for (int i = 0; i < 2; i++)
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            damage = GetDmg(s, i + 1),
                            fast = true,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = i + 1,
                    });
            return actions;
        }

        public override string Name() => "Immolate";
    }
}
