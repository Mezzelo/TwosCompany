using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MagmaInjection : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = true,
            };
        }
        private int GetHeatAmt(State s) {
            if (s.route is Combat) {
                return Math.Max(0, s.ship.Get(Status.heat) - 2);
            } else
                return 0;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B) {
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -2,
                    mode = AStatusMode.Add,
                    targetPlayer = true
                });
                actions.Add(new AVariableHint() {
                    status = Status.heat,
                });
                actions.Add(new AStatus() {
                    status = Status.corrode,
                    statusAmount = GetHeatAmt(s),
                    xHint = 1,
                    targetPlayer = false
                });
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true
                });
            }
            else {
                actions.Add(new AStatCostStatus() {
                    action = new AStatus() {
                        status = Status.corrode,
                        targetPlayer = false,
                        statusAmount = 1,
                    },
                    statusReq = Status.heat,
                    statusCost = upgrade == Upgrade.A ? 1 : 2,
                    cumulative = 0,
                    first = true,
                });
                actions.Add(new AStatCostStatus() {
                    action = new AStatus() {
                        status = Status.corrode,
                        targetPlayer = false,
                        statusAmount = 1,
                    },
                    statusReq = Status.heat,
                    statusCost = 2,
                    cumulative = upgrade == Upgrade.A ? 1 : 2,
                    first = false,
                });
            }
            return actions;
        }

        public override string Name() => "Searing Rupture";
    }
}
