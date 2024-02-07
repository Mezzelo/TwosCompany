using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Gravitate : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = upgrade != Upgrade.None,
                infinite = upgrade == Upgrade.B,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["GravitateCardSpriteFlip"] : Manifest.Sprites["GravitateCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }
        
        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.B ? 2 : 3,
                targetPlayer = false,
            });
            actions.Add(new ADroneMove() {
                dir = upgrade == Upgrade.B ? 1 : 2,
            });
            return actions;
        }

        public override string Name() => "Gravitate";
    }
}
