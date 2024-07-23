using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class DeusExMachina : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                buoyant = upgrade == Upgrade.A,
                exhaust = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["DefensiveFreeze"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                dialogueSelector = ".mezz_deusExMachina",
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.droneShift,
                statusAmount = 1
            });
            return actions;
        }

        public override string Name() => "Deus Ex Machina";
    }
}
