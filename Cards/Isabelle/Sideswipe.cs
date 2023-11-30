namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Sideswipe : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["SideswipeCardSpriteFlip"] : Manifest.Sprites["SideswipeCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                stunEnemy = upgrade == Upgrade.B
            });
            actions.Add(new AMove() {
                dir = -3,
                targetPlayer = true,
            });
            /*
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    targetPlayer = false,
                    status = Status.lockdown,
                    statusAmount = 1
                });
            */
            return actions;
        }

        public override string Name() => "Sideswipe";
    }
}
