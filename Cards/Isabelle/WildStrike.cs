namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
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
                damage = GetDmg(s, 1),
            });
            return actions;
        }

        public override string Name() => "Wild Strike";
    }
}
