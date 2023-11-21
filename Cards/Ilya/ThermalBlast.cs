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

            if (upgrade != Upgrade.None)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = upgrade == Upgrade.A ? -1 : 3,
                    targetPlayer = true,
                });
            actions.Add((CardAction)new AVariableHint() {
                status = new Status?(Status.heat)
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, this.GetHeatAmt(s)),
                xHint = 1
            });
            return actions;
        }

        public override string Name() => "Thermal Blast";
    }
}
