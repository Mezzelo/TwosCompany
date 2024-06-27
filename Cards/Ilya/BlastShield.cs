namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BlastShield : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                recycle = upgrade == Upgrade.B,
                retain = upgrade != Upgrade.B,
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 1;
            if (upgrade == Upgrade.None)
                heatAmt = 2;
            if (s.route is Combat) {
                heatAmt = Math.Max(0, s.ship.Get(Status.heat) + heatAmt + s.ship.Get(Status.boost));
            }
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = upgrade == Upgrade.None ? 2 : 1,
                targetPlayer = true
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = 1,
                    targetPlayer = true
                });
            actions.Add(new AVariableHint() {
                status = new Status?(Status.heat)
            });
            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = GetHeatAmt(s),
                xHint = 1,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "Blast Shield";
    }
}
