using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SummerBeforeFall : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 3 : 2,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["DefensiveFreeze"].Id!,
                    statusAmount = 2
                });
            else
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                    mode = AStatusMode.Set,
                    statusAmount = 3
                });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.powerdrive,
                statusAmount = 1,
                dialogueSelector = ".mezz_inDueTime",
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.energyLessNextTurn,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    brittle = true,
                });

            return actions;
        }

        public override string Name() => "Summer Before Fall";
    }
}
