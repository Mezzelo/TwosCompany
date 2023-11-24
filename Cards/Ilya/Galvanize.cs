namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Galvanize : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
            };
        }
        private int GetShieldAmt(State s) {
            int shieldAmt = 0;
            if (s.route is Combat)
                shieldAmt = s.ship.Get(Status.shield);
            return shieldAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add((CardAction)new AVariableHint() {
                status = new Status?(Status.shield)
            });

            if (upgrade == Upgrade.B) {
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = this.GetShieldAmt(s),
                    targetPlayer = true,
                    xHint = 1
                });
            }
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.B ? 0 : -3,
                mode = upgrade == Upgrade.B ? AStatusMode.Set : AStatusMode.Add,
                targetPlayer = true
            });
            actions.Add(new AStatus() {
                status = Status.maxShield,
                statusAmount = 2,
                targetPlayer = true
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AEnergy() {
                    changeAmount = 1
                });
            if (upgrade == Upgrade.A)
                actions.Add(new ADrawCard() {
                    count = 1
                });
            return actions;
        }

        public override string Name() => "Galvanize";
    }
}
