using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class InDueTime : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                exhaust = true,
            };
        }
        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses["BulletTime"].Id!,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                mode = AStatusMode.Set,
                targetPlayer = true,
            });
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.A ? 4 : 7,
                targetPlayer = true,
                isRandom = false
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 7),
                brittle = true,
                stunEnemy = true,
                dialogueSelector = ".mezz_inDueTime",
            });
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.A ? -4 : -7,
                targetPlayer = true,
                isRandom = false
            });
            return actions;
        }

        public override string Name() => "In Due Time";
    }
}
