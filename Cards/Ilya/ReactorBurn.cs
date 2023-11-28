namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ReactorBurn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade != Upgrade.A ? 0 : 1
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
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = this.GetHeatAmt(s),
                targetPlayer = true,
                xHint = 1
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -1,
                    targetPlayer = true,
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = this.GetHeatAmt(s),
                    xHint = 1
                });

            return actions;
        }

        public override string Name() => "Reactor Burn";
    }
}
