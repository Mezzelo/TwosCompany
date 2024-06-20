using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Contingency : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = upgrade == Upgrade.A,
                exhaust = true,
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
                dialogueSelector = ".mezz_contingency"
            });
            actions.Add(new ADrawCard() {
                count = GetHandSize(s),
                xHint = 1,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AEnergy() {
                    changeAmount = 1,
                });
            return actions;
        }

        public override string Name() => "Contingency";
    }
}
