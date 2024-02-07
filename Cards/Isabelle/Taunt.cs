namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Taunt : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                exhaust = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = false,
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true,
                dialogueSelector = ".mezz_taunt",
            });
            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Goad";
    }
}
