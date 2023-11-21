namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Cauterize : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.None ? 2 : 1,
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
                healAmount = upgrade == Upgrade.B ? 1 : 2,
                canRunAfterKill = true
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 3,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "Cauterize";
    }
}
