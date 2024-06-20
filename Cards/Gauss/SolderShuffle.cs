using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SolderShuffle : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
                flippable = true,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["SolderShuffleCardSpriteFlip"] : Manifest.Sprites["SolderShuffleCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AMove() {
                dir = -1,
                targetPlayer = true,
            });
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.normal,
                },
            });
            actions.Add(new AMove() {
                dir = -1,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.B) {
                actions.Add(new ASpawn() {
                    thing = new Midrow.Conduit() {
                        condType = Midrow.Conduit.ConduitType.normal,
                    },
                });
                actions.Add(new AMove() {
                    dir = -1,
                    targetPlayer = true,
                });
            }
            return actions;
        }

        public override string Name() => "Solder Shuffle";
    }
}
