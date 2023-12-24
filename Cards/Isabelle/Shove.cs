using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Shove : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["ShoveCardSpriteFlip"] : Manifest.Sprites["ShoveCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
                moveEnemy = upgrade == Upgrade.B ? 4 : 2
            });
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
            });
            actions.Add(new AFlipHandFixed());
            return actions;
        }

        public override string Name() => "Shove";
    }
}
