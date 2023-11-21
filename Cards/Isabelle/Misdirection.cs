namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Misdirection : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 0),
                    stunEnemy = true
                });
            actions.Add(new AFlipHand());
            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.None ? 1 : 2
            });

            return actions;
        }

        public override string Name() => "Misdirection";
    }
}
