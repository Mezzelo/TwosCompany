namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, dontOffer = true)]
    public class Ember : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 1,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Ember";
    }
}
