using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Wildfire : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Wildfire"].DescLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "left" : "right", flipped ? "right" : "left");
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Wildfire"].DescALocKey ?? throw new Exception("Missing card description")),
                    flipped ? "left" : "right", flipped ? "right" : "left");
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Wildfire"].DescBLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "left" : "right", flipped ? "right" : "left");

            return new CardData() {
                cost = 2,
                description = cardText,
                exhaust = true,
                retain = upgrade == Upgrade.B
            };
        }
        private int GetHandSize(State s) {
            int handSize = 0;
            if (s.route is Combat route)
                handSize = Math.Max(0, route.hand.Count - 1);
            return handSize;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADelay() {
                time = -0.5
            });
            actions.Add(new APlayAllCards() {
                leftToRight = false,
                gainHeat = 1,
                index = 0,
                timer = 0.5,
                firstPlay = true,
                dialogueSelector = GetHandSize(s) > 1 ? ".mezz_wildfire" : null
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -2,
                    targetPlayer = true
                });
            return actions;
        }

        public override void HilightOtherCards(State s, Combat c1) {
            foreach (Card card in c1.hand)
                if (card.uuid != this.uuid)
                    c1.hilightedCards.Add(card.uuid);
        }
        public override string Name() => "Wildfire";
    }
}