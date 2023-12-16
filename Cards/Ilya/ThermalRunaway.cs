using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ThermalRunaway : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                recycle = upgrade == Upgrade.B
            };
        }
        private int GetHandSize(State s) {
            int handSize = 0;
            if (s.route is Combat route)
                handSize = Math.Max(0, route.hand.Count - 1);
            return handSize;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AVariableHint() {
                hand = true,
                handAmount = this.GetHandSize(s)
            });
            actions.Add(new AExhaustEntireHandImmediate() {
                dialogueSelector = ".mezz_thermalRunaway"
            });
            actions.Add(new AAddCard() {
                amount = this.GetHandSize(s),
                card = new Ember(),
                xHint = 1,
                destination = CardDestination.Hand,
                timer = -0.5,
                waitBeforeMoving = 0
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -2,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Thermal Runaway";
    }
}
