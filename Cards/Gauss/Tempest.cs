using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Tempest : Card {

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Tempest"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Tempest"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Tempest"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
                description = cardText,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAddCard() {
                amount = 3,
                card = new SparkCard() { upgrade = Upgrade.None, temporaryOverride = true, discount = upgrade == Upgrade.B ? -1 : 0 },
                destination = upgrade == Upgrade.A ? CardDestination.Hand : CardDestination.Deck,
            });
            return actions;
        }

        public override string Name() => "Tempest";
    }
}
