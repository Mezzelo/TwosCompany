namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EnGarde : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                buoyant = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["EnGardeCardSpriteFlip"] : Manifest.Sprites["EnGardeCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.autododgeLeft,
                statusAmount = 1,
                targetPlayer = false
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = 2
                });
            return actions;
        }

        public override string Name() => "En Garde";
    }
}
