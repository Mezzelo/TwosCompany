using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Shatter : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Shatter"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Shatter"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Shatter"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = 1,
                retain = upgrade == Upgrade.A,
                exhaust = true,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AShatterFrozen() {
                onlyOutgoing = upgrade != Upgrade.B,
                weaken = upgrade != Upgrade.B,
                brittle = upgrade == Upgrade.B,
                dialogueSelector = ".mezz_shatter",
            });
            return actions;
        }

        public override string Name() => "Shatter";
    }
}
