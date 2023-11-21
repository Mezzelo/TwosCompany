namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Grapple : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.B ? 1 : 0),
                stunEnemy = upgrade == Upgrade.B,
                status = Status.lockdown,
                statusAmount = 1
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 2,
                    targetPlayer = true
                });
            else
                actions.Add(new AMove() {
                    dir = 2,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Grapple";
    }
}
