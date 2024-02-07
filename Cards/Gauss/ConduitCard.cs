using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ConduitCard : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                infinite = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.normal,
                    bubbleShield = upgrade == Upgrade.A,
                },
            });
            actions.Add(new AStatus() {
                status = Status.droneShift,
                statusAmount = 1,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Conduit";
    }
}
