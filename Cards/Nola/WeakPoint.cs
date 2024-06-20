namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class WeakPoint : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 3 : 4,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 0),
                piercing = true,
                stunEnemy = upgrade == Upgrade.B,
                brittle = true,
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                targetPlayer = true,
                statusAmount = 1,
            });
            return actions;
        }

        public override string Name() => "Weak Point";
    }
}
