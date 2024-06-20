using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class AllHands : Card {

        public bool wasPlayed = false;

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["AllHands"].DescLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "right" : "left", flipped ? "left" : "right");
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["AllHands"].DescALocKey ?? throw new Exception("Missing card description")),
                    flipped ? "right" : "left", flipped ? "left" : "right");
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["AllHands"].DescBLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "right" : "left", flipped ? "left" : "right");

            return new CardData() {
                cost = upgrade == Upgrade.B ? 3 : 4,
                description = cardText,
                flippable = upgrade == Upgrade.A,
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
                    leftToRight = !flipped,
                    gainHeat = 0,
                    index = 0,
                    timer = 0.5,
                    firstPlay = true,
                    dialogueSelector = GetHandSize(s) > 1 ? ".mezz_allHands" : null 
                });
            return actions;
        }
        public override void HilightOtherCards(State s, Combat c1) {
                foreach (Card card in c1.hand)
                    if (card.uuid != this.uuid)
                        c1.hilightedCards.Add(card.uuid);
        }
        public override void OnExitCombat(State s, Combat c) {
            wasPlayed = false;
        }

        public override void OnDraw(State s, Combat c) {
            wasPlayed = false;
        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (upgrade == Upgrade.B) {
                wasPlayed = true;
            }
        }

        public override void OnDiscard(State s, Combat c) {
        }

        public override string Name() => "All Hands";
    }
}
