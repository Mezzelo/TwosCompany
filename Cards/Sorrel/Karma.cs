using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class Karma : Card {
        public bool storyInf = false;
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Karma"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Karma"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Karma"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = upgrade == Upgrade.B ? 3 : 1,
                retain = upgrade == Upgrade.A,
                temporary = true,
                exhaust = true,
                infinite = storyInf,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade == Upgrade.B)
                actions.Add(new AForceAttack() {
                    // reverseAfter = true,
                    dialogueSelector = ".mezz_karma",
                });
            else
                actions.Add(new AReverseFrozen() {
                    omitFromTooltips = true,
                    dialogueSelector = ".mezz_karma",
                });
            return actions;
        }

        public override string Name() => "Karma";
    }
}
