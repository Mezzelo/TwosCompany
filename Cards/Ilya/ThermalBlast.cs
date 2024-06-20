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
            if (s.route is Combat) {
                heatAmt = s.ship.Get(Status.heat);
                if (upgrade == Upgrade.A)
                    heatAmt += 1 + s.ship.Get(Status.boost);
                else if (upgrade == Upgrade.B)
                    heatAmt += 2 + s.ship.Get(Status.boost);
            }
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade != Upgrade.None)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true,
                });
            actions.Add(new AVariableHint() {
                status = Status.heat
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, this.GetHeatAmt(s)),
                xHint = 1,
                dialogueSelector = this.GetHeatAmt(s) > 3 ? ".mezz_thermalBlast" : null
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = upgrade == Upgrade.A ? -2 : -1,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Thermal Blast";
    }
}
