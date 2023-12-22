using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ReactorBurn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                exhaust = upgrade != Upgrade.B
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat)
                heatAmt = s.ship.Get(Status.heat);
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();


            actions.Add((CardAction)new AVariableHint() {
                status = Status.heat
            });
            actions.Add(new AEnergy() {
                changeAmount = this.GetHeatAmt(s),
                xHint = 1
            });
            if (upgrade == Upgrade.B)
                actions.Add(new StatCostAction() {
                    action = new AHurt() {
                        hurtAmount = 1,
                        targetPlayer = true,
                        hurtShieldsFirst = false,
                    },
                    statusReq = Status.heat,
                    statusCost = 3,
                    first = true
                });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = this.GetHeatAmt(s),
                targetPlayer = true,
                xHint = 1
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -2,
                    targetPlayer = true,
                });

            if (upgrade != Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = Math.Max(0, this.GetHeatAmt(s) - (upgrade == Upgrade.A ? 2 : 0)),
                    xHint = 1,
                });
            return actions;
        }

        public override string Name() => "Reactor Burn";
    }
}
