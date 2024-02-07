using System;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ReversePolarity : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }
        private int GetEvadeAmt(State s) {
            int evadeAmt = 0;
            if (s.route is Combat) {
                evadeAmt = s.ship.Get(Status.droneShift);
            }
            return evadeAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AVariableHint() {
                status = Status.droneShift,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAddCard() {
                    amount = GetEvadeAmt(s),
                    card = new KineticConduit() { upgrade = Upgrade.None, discount = -1, temporaryOverride = true, exhaustOverride = true },
                    destination = CardDestination.Hand,
                    xHint = 1,
                });
            else
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = GetEvadeAmt(s),
                    targetPlayer = true,
                    xHint = upgrade == Upgrade.B ? 2 : 1,
                });
            actions.Add(new AStatus() {
                status = Status.droneShift,
                statusAmount = upgrade == Upgrade.A ? 1 : 0,
                mode = AStatusMode.Set,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Reverse Polarity";
    }
}
