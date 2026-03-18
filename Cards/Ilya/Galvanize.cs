namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Galvanize : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                buoyant = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 3,
                targetPlayer = true,
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = upgrade == Upgrade.A ? 1 : 2,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Galvanize";
    }
}
