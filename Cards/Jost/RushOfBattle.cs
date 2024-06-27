using TwosCompany.Actions;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RushOfBattle : Card {

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["RushOfBattle"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["RushOfBattle"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["RushOfBattle"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = cardText,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAddCard() {
                amount = 3,
                card = new Heartbeat() { upgrade = Upgrade.None, temporaryOverride = true, retainOverride = upgrade == Upgrade.B, breatheInRetain = upgrade == Upgrade.B },
                destination = CardDestination.Hand,
                showCardTraitTooltips = false,
            });
            return actions;
        }

        public override string Name() => "Rush Of Battle";
    }
}
