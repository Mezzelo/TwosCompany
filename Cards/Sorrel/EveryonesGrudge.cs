using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EveryonesGrudge : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["EveryonesGrudge"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["EveryonesGrudge"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["EveryonesGrudge"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = 3,
                exhaust = true,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = 2,
                mode = AStatusMode.Set,
            });
            actions.Add(new AAddCard() {
                card = new Karma() { upgrade = this.upgrade, },
                destination = CardDestination.Deck,
                amount = 1,
                handPosition = 0,
                insertRandomly = false,
                timer = 0.2,
            });
            return actions;
        }

        public override string Name() => "Everyone's Grudge";
    }
}
