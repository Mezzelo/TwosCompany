using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FrozenInMotion : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 2,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 3,
                mode = AStatusMode.Set,
                dialogueSelector = ".mezz_raisedPalm",
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.droneShift,
                statusAmount = 2,
            });
            actions.Add(new AStatus() {
                targetPlayer = false,
                status = Status.lockdown,
                statusAmount = upgrade == Upgrade.A ? 3 : (upgrade == Upgrade.B ? 1 : 2),
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.lockdown,
                statusAmount = upgrade == Upgrade.B ? 1 : 2
            });

            return actions;
        }

        public override string Name() => "World in Motion";
    }
}
