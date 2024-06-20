namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Riposte : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.autododgeRight,
                statusAmount = 2,
                targetPlayer = true
            });
            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?["TempStrafe"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                mode = AStatusMode.Add,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.stunCharge,
                    statusAmount = 2,
                    mode = AStatusMode.Add,
                    targetPlayer = true,
                });
            actions.Add(new AStatus() {
                status = Status.lockdown,
                statusAmount = 1,
                mode = AStatusMode.Add,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Riposte";
    }
}
