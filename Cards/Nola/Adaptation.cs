namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Adaptation : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                floppable = true,
                infinite = true,
                art = new Spr?((Spr) ((flipped ? Manifest.Sprites["AdaptationCardSpriteFlip"] : Manifest.Sprites["AdaptationCardSprite"]).Id 
                    ?? throw new Exception("missing flop art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.evade,
                targetPlayer = true,
                statusAmount = -1,
                disabled = flipped
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                targetPlayer = true,
                statusAmount = 3,
                disabled = flipped
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    targetPlayer = true,
                    statusAmount = 1,
                    disabled = flipped
                });
            actions.Add(new ADummyAction());
            actions.Add(new AStatus() {
                status = Status.shield,
                targetPlayer = true,
                statusAmount = upgrade == Upgrade.B ? -2 : -1,
                disabled = !flipped
            });
            actions.Add(new AStatus() {
                status = Status.evade,
                targetPlayer = true,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                disabled = !flipped
            });

            return actions;
        }

        public override string Name() => "Adaptation";
    }
}
