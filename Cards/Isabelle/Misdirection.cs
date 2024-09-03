using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Misdirection : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = upgrade != Upgrade.None,
                infinite = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AFlipHandFixed());
            if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = 1
                });
            else {
                actions.Add(new AStatus() {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            }

            return actions;
        }

        public override string Name() => "Misdirection";
    }
}
