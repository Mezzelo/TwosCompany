namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MeasureBreak : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 0),
                fast = true,
                stunEnemy = upgrade == Upgrade.B
            });
            actions.Add(new AMove() {
                dir = 3,
                targetPlayer = true,
                isRandom = true
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 0 : 2),
                fast = true,
                stunEnemy = upgrade != Upgrade.B
            });
            return actions;
        }

        public override string Name() => "Measure Break";
    }
}
