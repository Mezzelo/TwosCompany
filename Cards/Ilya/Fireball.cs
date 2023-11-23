namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Fireball : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                infinite = upgrade == Upgrade.B
            };
        }
        private int GetHandSize(State s) {
            int handSize = 0;
            if (s.route is Combat route)
                handSize = Math.Max(0, route.hand.Count - 3);
            return handSize;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADiscard() {
                count = 2,
            });

            actions.Add(new AVariableHint() {
                hand = true,
                handAmount = this.GetHandSize(s)
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, this.GetHandSize(s) * (upgrade == Upgrade.A ? 2 : 1)),
                xHint = upgrade == Upgrade.A ? 2 : 1,
                fast = upgrade != Upgrade.A,
            });
            if (upgrade != Upgrade.A)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, this.GetHandSize(s)),
                    fast = true,
                    xHint = 1
                });

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = this.GetHandSize(s),
                targetPlayer = true,
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

        public override string Name() => "Fireball";
    }
}
