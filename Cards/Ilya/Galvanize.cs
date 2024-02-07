namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Galvanize : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }
        private int GetShieldAmt(State s) {
            int shieldAmt = 0;
            if (s.route is Combat)
                shieldAmt = s.ship.Get(Status.shield)
                    + (upgrade == Upgrade.A ? 1 : (upgrade == Upgrade.B ? s.ship.Get(Status.tempShield) : 0));

            return shieldAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.A) {
                actions.Add(new AStatus() {
                    status = Status.shield,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            }
            actions.Add(new AVariableHint() {
                status = Status.shield,
                secondStatus = upgrade == Upgrade.B ? Status.tempShield : null
            });

            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = this.GetShieldAmt(s),
                targetPlayer = true,
                xHint = 1
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = this.GetShieldAmt(s),
                targetPlayer = true,
                xHint = 1
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -1,
                    targetPlayer = true,
                });
            }
            return actions;
        }

        public override string Name() => "Galvanize";
    }
}
