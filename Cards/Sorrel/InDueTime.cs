using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class InDueTime : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = true,
            };
        }
        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses["BulletTime"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 3,
                mode = AStatusMode.Set,
                targetPlayer = true,
            });
            actions.Add(new AStatus() {
                status = Status.droneShift,
                statusAmount = upgrade == Upgrade.B ? 1 : 0,
                mode = AStatusMode.Set,
                targetPlayer = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 9 : 6),
                stunEnemy = true,
                dialogueSelector = ".mezz_inDueTime",
            });
            return actions;
        }

        public override string Name() => "In Due Time";
    }
}
