using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class AsteroidBelt : Card {

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["AsteroidBelt"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["AsteroidBelt"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["AsteroidBelt"].DescBLocKey ?? throw new Exception("Missing card description"));
            return new CardData() {
                cost = 2,
                description = cardText,
                exhaust = upgrade == Upgrade.B,
                retain = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAsteroidBelt() {
                bubbled = upgrade == Upgrade.B
            });
            return actions;
        }

        public override string Name() => "Asteroid Belt";
    }
}
