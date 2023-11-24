using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Foresight : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["Foresight"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["Foresight"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["Foresight"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 1,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            /*
            actions.Add(new ADelay() {
                time = -0.5
            });*/

            int drawSize = 0;
            int iMax = 4;
            if (s.route is Combat)
                drawSize = s.deck.Count;

            if (upgrade == Upgrade.B) {
                if (drawSize > 1)
                    actions.Add(new ACardSelect() {
                        browseAction = new AForesight() {
                            exhaust = true,
                            timer = -0.5
                        },
                        browseSource = CardBrowse.Source.DrawPile

                    });
                else if (drawSize > 0)
                    actions.Add(new AForesight() {
                        exhaust = true,
                        timer = -0.5,
                        deckIndex = 0
                    });
                iMax--;
                drawSize--;
            }
            for (int i = iMax - 1; i > -1; i--) {
                if (drawSize > iMax)
                    actions.Add(new ACardSelect() {
                        browseAction = new AForesight() {
                            exhaust = false,
                            timer = -0.5
                        },
                        browseSource = CardBrowse.Source.DrawPile
                    });
                else if (drawSize >= 0)
                    actions.Add(new AForesight() {
                        exhaust = false,
                        timer = -0.5,
                        deckIndex = i
                    });
            }

            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.A ? 3 : 1
            });
            return actions;
        }

        public override string Name() => "Foresight";
    }
}
