using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CaptainsOrders : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["CaptainsOrders"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["CaptainsOrders"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["CaptainsOrders"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 3,
                description = cardText,
                retain = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADelay() {
                time = -0.5
            });

            actions.Add(new ACardSelect() {
                browseAction = new ACaptainsOrders() {
                    dontExhaust = upgrade == Upgrade.B,
                    dialogueSelector = ".mezz_captainsOrders"
                },
                browseSource = CardBrowse.Source.Hand
            });
            if (upgrade == Upgrade.A)
                actions.Add(new ADrawCard() {
                    count = 2
                });
            return actions;
        }

        public override string Name() => "Captain's Orders";
    }
}
