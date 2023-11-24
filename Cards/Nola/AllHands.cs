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
                    flipped ? "rightmost" : "leftmost");

            return new CardData() {
                cost = wasPlayed && upgrade == Upgrade.B ? 0 : 3,
                description = cardText,
                flippable = upgrade == Upgrade.A,
                infinite = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADelay() {
                time = -0.5
            });
            if (upgrade == Upgrade.B && s.route is Combat route) {
                bool preventPlay = false;
                bool doubleInf = false;
                int handPosition = flipped ? c.hand.Count - 1 : 0;
                if (c.hand.Count == 1) {
                    actions.Add(new ADiscard());
                } else {
                    if (!flipped && this.uuid == c.hand[0].uuid)
                        handPosition += 1;
                    else if (flipped && this.uuid == c.hand[c.hand.Count - 1].uuid)
                        handPosition -= 1;
                    if (this.GetFullDisplayName().Equals(c.hand[handPosition].GetFullDisplayName()))
                        preventPlay = true;
                    if (c.hand[handPosition].GetData(s).infinite)
                        doubleInf = true;
                }
                if (preventPlay)
                    actions.Add(new ADiscardSpecific() {
                        selectedCard = this,
                        drawNotDiscard = false,
                        discount = 0
                    });
                else
                    actions.Add(new APlayOtherCard() {
                        handPosition = handPosition,
                        timer = 0.5,
                        exhaustThisCardAfterwards = false,
                    });
                if (doubleInf)
                    actions.Add(new ADiscardSpecific() {
                        selectedCard = this,
                        drawNotDiscard = false,
                        discount = 0
                    });
            }
            else
                actions.Add(new APlayAllCards() {
                    leftToRight = !flipped,
                    gainHeat = 0,
                    index = 0,
                    timer = 0.5,
                    firstPlay = true
                });
            return actions;
        }
        public override void HilightOtherCards(State s, Combat c1) {
            if (upgrade == Upgrade.B) {
                var card = Enumerable.FirstOrDefault(Enumerable.Where(c1.hand, c2 => c2 != this));
                if (card == null)
                    return;
                c1.hilightedCards.Add(card.uuid);
            } else {
                foreach (Card card in c1.hand)
                    if (card.uuid != this.uuid)
                        c1.hilightedCards.Add(card.uuid);
            }
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
