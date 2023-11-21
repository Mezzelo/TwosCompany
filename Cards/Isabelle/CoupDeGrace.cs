namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CoupDeGrace : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 4,
                exhaust = upgrade != Upgrade.B,
                recycle = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade != Upgrade.A ? 6 : 8),
            });

            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 0,
                targetPlayer = true,
                mode = AStatusMode.Set,
                omitFromTooltips = true
            });
            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = 0,
                targetPlayer = true,
                mode = AStatusMode.Set,
                omitFromTooltips = true
            });
            actions.Add(new AStatus() {
                status = Status.lockdown,
                statusAmount = upgrade != Upgrade.B ? 3 : 2,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "Coup De Grace";
    }
}
