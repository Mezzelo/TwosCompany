using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LetLoose : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["LetLoose"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["LetLoose"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["LetLoose"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 1,
                description = cardText,
                exhaust = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            
            if (upgrade == Upgrade.A)
                actions.Add(new ADrawCard() {
                    count = 2,
                });

            actions.Add(new ALowerCardCost() {
                amount = upgrade == Upgrade.B ? -2 : -1,
                hand = true,
                minimum = upgrade == Upgrade.B ? -1 : 1,
                // dialogueSelector = ".mezz_letLoose"
            });

            return actions;
        }

        public override string Name() => "Let Loose";
    }
}
