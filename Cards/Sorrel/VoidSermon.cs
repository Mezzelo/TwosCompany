using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class VoidSermon : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["VoidSermon"].DescLocKey ?? throw new Exception("Missing card description")),
                   "Double");
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["VoidSermon"].DescALocKey ?? throw new Exception("Missing card description")),
                    "Double");
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["VoidSermon"].DescBLocKey ?? throw new Exception("Missing card description")),
                    "Triple");

            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
                exhaust = true,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADoubleFrozen() {
                mult = upgrade == Upgrade.B ? 3 : 2,
                dialogueSelector = ".mezz_shatter",
            });
            return actions;
        }

        public override string Name() => "Void Sermon";
    }
}
