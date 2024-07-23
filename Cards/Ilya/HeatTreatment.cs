namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HeatTreatment : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = true,
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat)
                heatAmt = Math.Max(0, s.ship.Get(Status.heat) - (upgrade == Upgrade.B ? 0 : 2));
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = upgrade == Upgrade.B ? -1 : -2,
                targetPlayer = true
            });
            actions.Add(new AVariableHint() {
                status = Status.heat,
            });
            actions.Add(new AHeal() {
                targetPlayer = true,
                healAmount = GetHeatAmt(s),
                xHint = 1,
                canRunAfterKill = true
            });
            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 2,
                    targetPlayer = true
                });
            return actions;
        }

        public override string Name() => "Heat  Treatment";
    }
}
