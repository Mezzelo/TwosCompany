namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Haze : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat)
                heatAmt = s.ship.Get(Status.heat) + (upgrade == Upgrade.A ? 1 : 0);
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();


            actions.Add(new AMove() {
                dir = upgrade == Upgrade.A ? 1 : 2,
                isRandom = true,
                targetPlayer = true
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            actions.Add((CardAction)new AVariableHint() {
                status = Status.heat
            });
            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = this.GetHeatAmt(s),
                targetPlayer = true,
                xHint = 1
            });

            return actions;
        }

        public override string Name() => "Haze";
    }
}
