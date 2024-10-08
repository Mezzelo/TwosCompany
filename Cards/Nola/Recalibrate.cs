using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Recalibrate : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["Recalibrate"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["Recalibrate"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["Recalibrate"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 1,
                description = cardText,
                retain = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADiscardShuffle());
            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.B ? 5 : 3,
                // dialogueSelector = ".mezz_recalibrate"
            });
            return actions;
        }

        public override string Name() => "Recalibrate";
    }
}
