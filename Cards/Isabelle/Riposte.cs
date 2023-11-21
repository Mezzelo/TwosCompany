namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Riposte : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = 1,
                targetPlayer = true
            });
            actions.Add(new AStatus() {
                status = Status.tempPayback,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = 2,
                    targetPlayer = true
                });
            return actions;
        }

        public override string Name() => "Riposte";
    }
}
