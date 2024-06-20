namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Burnout : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = true,
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
                status = new Status?(Status.heat)
            });
            actions.Add(new ADrawCard() {
                count = GetHeatAmt(s) * (upgrade == Upgrade.A ? 3 : 2),
                xHint = upgrade == Upgrade.A ? 3 : 2
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = upgrade == Upgrade.B ? 2 : 0,
                mode = AStatusMode.Set,
                targetPlayer = true
            });

            return actions;
        }

        public override string Name() => "Burnout";
    }
}
