namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Cauterize : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = upgrade != Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = upgrade != Upgrade.A ? Status.shield : Status.tempShield,
                statusAmount = upgrade != Upgrade.A ? 2 : 4,
                targetPlayer = true
            });
            actions.Add(new AHeal() {
                targetPlayer = true,
                healAmount = 1,
                canRunAfterKill = true
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 3,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AEndTurn());
            return actions;
        }

        public override string Name() => "Cauterize";
    }
}
