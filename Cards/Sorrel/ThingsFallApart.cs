using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ThingsFallApart : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = upgrade == Upgrade.A,
                infinite = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                mode = AStatusMode.Set,
                statusAmount = 1
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1),
                stunEnemy = upgrade != Upgrade.B,
                dialogueSelector = ".mezz_morningDew",
            });
            if (upgrade == Upgrade.A)
                actions.Add(new ADroneMove() {
                    dir = 1,
                });

            return actions;
        }

        public override string Name() => "Things Fall Apart";
    }
}
