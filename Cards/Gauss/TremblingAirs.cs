using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TremblingAirs : Card {

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["TremblingAirs"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["TremblingAirs"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["TremblingAirs"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = upgrade == Upgrade.A ? 0 : 1,
                exhaust = upgrade == Upgrade.B,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ASearchForChain() {
                discount = upgrade == Upgrade.B ? 1 : 0,
                dialogueSelector = ".mezz_tremblingAirs",
            });
            return actions;
        }

        public override string Name() => "Trembling Airs";
    }
}
