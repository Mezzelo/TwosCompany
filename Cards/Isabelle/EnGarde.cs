namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EnGarde : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 3,
                buoyant = upgrade == Upgrade.A,
                flippable = upgrade == Upgrade.B,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["EnGardeCardSpriteFlip"] : Manifest.Sprites["EnGardeCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.tempPayback,
                statusAmount = 1,
                targetPlayer = false
            });
            actions.Add(new AStatus() {
                status = flipped ? Status.autododgeRight : Status.autododgeLeft,
                statusAmount = 1,
                targetPlayer = true
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
            });
            return actions;
        }

        public override string Name() => "En Garde";
    }
}
