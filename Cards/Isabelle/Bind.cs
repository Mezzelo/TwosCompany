namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Bind : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.shield : Status.tempShield,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true
            });
            actions.Add(new AStatus() {
                status = Status.engineStall,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "Bind";
    }
}
