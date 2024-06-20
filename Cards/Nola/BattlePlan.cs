namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BattlePlan : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["BattlePlan"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["BattlePlan"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["BattlePlan"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 1,
                description = cardText,
                retain = upgrade == Upgrade.A,
                exhaust = true
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADelay() {
                time = -0.5
            });
            if (s.route is Combat)
                foreach (Card current in c.hand) {
                    if (current.uuid == this.uuid)
                        continue;
                    Card newCard = current.CopyWithNewId();
                    newCard.temporaryOverride = true;
                    newCard.exhaustOverride = true;
                    actions.Add(new AAddCard() {
                        card = newCard,
                        callItTheDeckNotTheDrawPile = false,
                        insertRandomly = false,
                        amount = 1,
                        showCardTraitTooltips = false,
                        waitBeforeMoving = 0.0
                    });
                }

            return actions;
        }

        public override string Name() => "Battle Plan";
    }
}
