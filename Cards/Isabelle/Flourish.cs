using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Flourish : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 2,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["FlourishCardSpriteFlip"] : Manifest.Sprites["FlourishCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
            });
            actions.Add(new AMove() {
                dir = 2,
                targetPlayer = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                fast = true,
            });
            actions.Add(new AFlipHandFixed());
            return actions;
        }

        public override string Name() => "Flourish";
    }
}
