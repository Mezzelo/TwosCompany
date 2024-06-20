using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ShieldConduit : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                flippable = upgrade == Upgrade.B,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["ShieldConduitCardSpriteFlip"] : Manifest.Sprites["ShieldConduitCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.shield,
                    bubbleShield = upgrade == Upgrade.A,
                },
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ASpawn() {
                    thing = new ShieldDrone() {
                        targetPlayer = true,
                        bubbleShield = true,
                    },
                    offset = 1,
                });
            return actions;
        }

        public override string Name() => "Shield Conduit";
    }
}
