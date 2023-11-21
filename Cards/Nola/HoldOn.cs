namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HoldOn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.hermes,
                statusAmount = upgrade == Upgrade.A ? 2 : 3
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus()
                {
                    targetPlayer = true,
                    status = Status.evade,
                    statusAmount = 1
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new AStatus()
                {
                    targetPlayer = true,
                    status = Status.strafe,
                    statusAmount = 1
                });
            return actions;
        }

        public override string Name() => "Hold On!";
    }
}
