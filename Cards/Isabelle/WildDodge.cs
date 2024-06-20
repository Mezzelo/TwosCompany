namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, dontOffer = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, unreleased = true)]
    public class WildDodge : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new AMove() {
                    dir = 1,
                    targetPlayer = true,
                    isRandom = true
                });
            if (upgrade != Upgrade.None)
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = Status.evade,
                    statusAmount = 1
                });
            actions.Add(new AMove() {
                dir = 1,
                targetPlayer = true,
                isRandom = true
            });
            return actions;
        }

        public override string Name() => "Wild Dodge";
    }
}
