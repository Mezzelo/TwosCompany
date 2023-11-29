namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BlastShield : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = true
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat)
                heatAmt = Math.Max(0, s.ship.Get(Status.heat) + (upgrade == Upgrade.A ? 0 : 2 + s.ship.Get(Status.boost)));
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 2,
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
            if (upgrade != Upgrade.None)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -2,
                    targetPlayer = true
                });
            return actions;
        }

        public override string Name() => "Blast Shield";
    }
}
