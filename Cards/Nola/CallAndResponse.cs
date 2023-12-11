using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CallAndResponse : Card {

        public Card? storedCard;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = upgrade == Upgrade.B,
                art = storedCardState(state) ? (storedCard ?? throw new Exception("bad null check ig, c&r")).GetArt(state) :
                    new Spr?((Spr)(Manifest.Sprites["CallAndResponseCardSprite"].Id
                    ?? throw new Exception("missing c&r art"))),
                unplayable = storedInHand(state),
            };
        }
        private bool storedInHand(State s) {
            if (storedCard == null)
                return false;
            if (s.route is Combat route) {
                foreach (Card search in route.hand)
                    if (search.uuid == storedCard.uuid)
                        return true;
            }
            return false;
        }
        private bool storedCardState(State s) {
            if (storedCard == null)
                return false;
            if (s.route is Combat route) {
                foreach (Card search in route.exhausted)
                    if (search.uuid == storedCard.uuid)
                        return false;

                if (s.FindCard(storedCard.uuid) == null)
                    return false;
            }
            return true;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();


            if (!storedCardState(s)) {
                actions.Add(new ADelay() {
                    time = -0.5
                });
                
                actions.Add(new ACardSelect() {
                    browseAction = new ACallAndResponse() {
                        callCard = this,
                        recall = false,
                    },
                    browseSource = CardBrowse.Source.Hand
                });
                actions.Add(new ACallAndResponse() {
                    recall = false,
                    discount = upgrade == Upgrade.A ? 2 : 1
                });
            }
            else {
                actions.Add(new ACallAndResponse() {
                    selectedCard = this.storedCard,
                    recall = true,
                    discount = upgrade == Upgrade.A ? 2 : 1
                });
            }
            if (!storedCardState(s))
                actions.Add(new ADummyAction());
            return actions;
        }
        public override void OnExitCombat(State s, Combat c) {
            storedCard = null;
        }

        public override string Name() => "Call and Response";
    }
}
