using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Ruminate : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["Ruminate"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["Ruminate"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["Ruminate"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 1,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADelay() {
                time = -0.5
            });

            for (int i = 0; i < 2; i++)
                actions.Add(new ACardSelect() {
                    browseAction = new ADiscardSpecific() {
                        drawNotDiscard = true,
                        discount = upgrade == Upgrade.B ? -1 : 0,
                    },
                    browseSource = CardBrowse.Source.Hand
                });

            if (upgrade == Upgrade.A)
            actions.Add(new AStatus() {
                status = Status.drawNextTurn,
                statusAmount = 2,
                targetPlayer = true
            });
            actions.Add(new AEndTurn());
            return actions;
        }

        public override string Name() => "Ruminate";
    }
}
