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
                statusAmount = 2,
                targetPlayer = false,
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true,
                dialogueSelector = ".mezz_taunt",
            });
            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = upgrade == Upgrade.A ? -2 : 0,
                mode = upgrade == Upgrade.A ? AStatusMode.Add : AStatusMode.Set,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Goad";
    }
}
