using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Pressure : Card {

        public bool isTemp = false;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                exhaust = upgrade == Upgrade.B,
                temporary = isTemp
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 1,
                targetPlayer = true,
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 3,
                targetPlayer = false,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAddCard() {
                    card = new Pressure() { upgrade = Upgrade.B, isTemp = true },
                    destination = CardDestination.Deck,
                    showCardTraitTooltips = true
                });
            actions.Add(new ADrawCard() {
                count = upgrade != Upgrade.None ? 2 : 1
            });
            return actions;
        }

        public override string Name() => "Pressure";
    }
}
