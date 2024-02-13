using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Autocurrent : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 2 : 3,
                exhaust = upgrade != Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses["Autocurrent"].Id!,
                statusAmount = 1,
                targetPlayer = true,
                dialogueSelector = ".mezz_autocurrent",
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 1),
                });
            return actions;
        }

        public override string Name() => "Autocurrent";
    }
}
