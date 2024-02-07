using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Misdirection : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
                retain = upgrade == Upgrade.A,
                infinite = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AFlipHandFixed());
            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.B ? 3 : 1
            });

            return actions;
        }

        public override string Name() => "Misdirection";
    }
}
