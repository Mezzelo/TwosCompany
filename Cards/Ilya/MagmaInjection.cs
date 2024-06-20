namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MagmaInjection : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 2 : 3,
                exhaust = true,
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat) {
                heatAmt = s.ship.Get(Status.heat);
                if (upgrade == Upgrade.B)
                    heatAmt += s.ship.Get(Status.overdrive);
            }
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AVariableHint() {
                status = Status.heat,
                secondStatus = upgrade == Upgrade.B ? Status.overdrive : null
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
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.overdrive,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true
                });
            return actions;
        }

        public override string Name() => "Searing Rupture";
    }
}
