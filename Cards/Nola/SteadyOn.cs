using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SteadyOn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.evade,
                targetPlayer = true,
                statusAmount = upgrade == Upgrade.A ? -1 : -2,
            });
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.evade,
                statusCost = 1,
                cumulative = upgrade == Upgrade.A ? 1 : 2
            });
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.evade,
                statusCost = 1,
                cumulative = upgrade == Upgrade.A ? 2 : 3
            });
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.evade,
                statusCost = 1,
                cumulative = upgrade == Upgrade.A ? 3 : 4
            });
            /*
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.hermes,
                    targetPlayer = true,
                    statusAmount = 1,
                });
            */
            return actions;
        }

        public override string Name() => "Steady On";
    }
}
