using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ConductorArray : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["ConductorArrayCardSpriteFlip"] : Manifest.Sprites["ConductorArrayCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.normal,
                },
                offset = upgrade == Upgrade.B ? -1 : 1,
            });
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.normal,
                },
                offset = upgrade == Upgrade.B ? 1 : 2,
            });
            actions.Add(new AStatus() {
                status = Status.droneShift,
                statusAmount = 1,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "ConductorArray";
    }
}
