using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Anticipation : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["Anticipation"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["Anticipation"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["Anticipation"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 2,
                description = cardText,
                recycle = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADelay() {
                time = -0.5
            });

            int drawSize = 1;
            if (s.route is Combat route)
                drawSize = s.deck.Count;
            if (drawSize > 1) {
                actions.Add(new ACardSelect() {
                    browseAction = new ALowerCardCost() {
                        amount = upgrade == Upgrade.A ? -2 : -4,
                        minimum = -1
                    },
                    browseSource = CardBrowse.Source.DrawPile
                });
                if (upgrade == Upgrade.A)
                    actions.Add(new ACardSelect() {
                        browseAction = new ALowerCardCost() {
                            amount = -2,
                            minimum = -1
                        },
                        browseSource = CardBrowse.Source.DrawPile
                    });
            } else if (drawSize == 1) {
                actions.Add(new ALowerCardCost() {
                    amount = upgrade == Upgrade.A ? -2 : -4,
                    selectedIndex = 0,
                    minimum = -1
                });
                if (upgrade == Upgrade.A)
                    actions.Add(new ALowerCardCost() {
                        amount = -2,
                        selectedIndex = 0,
                        minimum = -1
                    });
            }
            return actions;
        }

        public override string Name() => "Anticipation";
    }
}
