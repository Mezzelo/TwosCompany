namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Riposte : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade != Upgrade.B)
            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = 1,
                targetPlayer = true
            });
            actions.Add(new AStatus() {
                status = Status.tempPayback,
                statusAmount = upgrade == Upgrade.A ? 3 : 2,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B)
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
