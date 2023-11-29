namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, dontOffer = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class WildStrike : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
                stunEnemy = upgrade == Upgrade.B
            });
            return actions;
        }

        public override string Name() => "Wild Strike";
    }
}
