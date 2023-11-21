namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Apex : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade != Upgrade.A ? 1 : 3,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            int freezeAmount = upgrade == Upgrade.None ? 2 : 1;
            if (upgrade == Upgrade.B)
                freezeAmount = 3;
            actions.Add(new AStatus() {
                status = Status.lockdown,
                statusAmount = freezeAmount,
                targetPlayer = false
            });
            actions.Add(new AStatus() {
                status = Status.lockdown,
                statusAmount = freezeAmount,
                targetPlayer = true
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            actions.Add(new AStatus() {
                status = Status.powerdrive,
                statusAmount = upgrade != Upgrade.B ? 1 : 2,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Apex";
    }
}
