namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Maul : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade != Upgrade.B ? 2 : 3,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                fast = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                fast = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 3),
                fast = true,
            });
            actions.Add(new AStatus() {
                status = Status.drawLessNextTurn,
                statusAmount = upgrade != Upgrade.A ? 4 : 2,
                targetPlayer = true
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AEndTurn());
            return actions;
        }

        public override string Name() => "Maul";
    }
}
