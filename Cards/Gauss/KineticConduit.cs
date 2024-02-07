using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class KineticConduit : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.kinetic,
                },
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 0),
                });
            return actions;
        }

        public override string Name() => "Kinetic Conduit";
    }
}
