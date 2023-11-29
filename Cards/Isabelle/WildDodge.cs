namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, dontOffer = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
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
                    dir = 2,
                    targetPlayer = true,
                    isRandom = true
                });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = upgrade != Upgrade.None ? 2 : 1
            });
            return actions;
        }

        public override string Name() => "Wild Dodge";
    }
}
