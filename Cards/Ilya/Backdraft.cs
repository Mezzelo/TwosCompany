namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Backdraft : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["BackdraftCardSpriteFlip"] : Manifest.Sprites["BackdraftCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADroneMove() {
                dir = 2,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.B ? 1 : 2),
                stunEnemy = upgrade == Upgrade.B
            });
            if (upgrade == Upgrade.B) {
                actions.Add(new AMove() {
                    dir = 1,
                    targetPlayer = true,
                });
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    stunEnemy = true
                });
            }
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true,
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -1,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Backdraft";
    }
}
