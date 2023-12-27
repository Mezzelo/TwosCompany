namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CoupDeGrace : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = upgrade != Upgrade.B,
                recycle = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 7),
                stunEnemy = upgrade == Upgrade.A,
                dialogueSelector = ".mezz_coupDeGrace",
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
                statusAmount = 1,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "Coup De Grace";
    }
}
