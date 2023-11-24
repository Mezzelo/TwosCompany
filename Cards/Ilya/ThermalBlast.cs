namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ThermalBlast : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
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

            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 3,
                    targetPlayer = true,
                });
            actions.Add(new AVariableHint() {
                status = new Status?(Status.heat)
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, this.GetHeatAmt(s)),
                xHint = 1
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -1,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Thermal Blast";
    }
}
