namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Haymaker : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 3 : 2,
                exhaust = true,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["HaymakerCardSpriteFlip"] : Manifest.Sprites["HaymakerCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AMove() {
                dir = -6,
                targetPlayer = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 5),
                weaken = upgrade != Upgrade.B,
                brittle = upgrade == Upgrade.B,
                stunEnemy = true
            });
            actions.Add(new AStatus() {
                status = Status.drawLessNextTurn,
                statusAmount = 1,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "Haymaker";
    }
}
