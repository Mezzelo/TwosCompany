namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BladeDance : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["BladeDance"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["BladeDance"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["BladeDance"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 1,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAddCard() {
                amount = upgrade == Upgrade.B ? 3 : 2,
                card = new Flourish() { discount = -1, exhaustOverride = true, temporaryOverride = true, 
                    upgrade = (this.upgrade == Upgrade.A ? Upgrade.A : Upgrade.None) },
                destination = CardDestination.Hand,
                timer = 0.2,
                waitBeforeMoving = 0.2
            });
            return actions;
        }

        public override string Name() => "Blade Dance";
    }
}
