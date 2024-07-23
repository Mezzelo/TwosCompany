using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Collapse : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Collapse"].DescLocKey ?? throw new Exception("Missing card description")),
                   flipped ? "right" : "left");
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Collapse"].DescALocKey ?? throw new Exception("Missing card description")),
                    flipped ? "right" : "left");
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Collapse"].DescBLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "right" : "left");

            return new CardData() {
                cost = 1,
                flippable = upgrade == Upgrade.A,
                retain = upgrade == Upgrade.B,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ACollapseFrozen() {
                dir = flipped ? 1 : -1,
                omitFromTooltips = true,
            });
            return actions;
        }

        public override string Name() => "Waterfall";
    }
}
