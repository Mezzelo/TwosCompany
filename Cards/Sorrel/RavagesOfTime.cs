using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RavagesOfTime : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
            };
        }
        private int GetBTimeAmt(State s) {
            int bTimeAmt = 0;
            if (s.route is Combat) {
                bTimeAmt = s.ship.Get((Status) Manifest.Statuses["BulletTime"].Id!) * (upgrade == Upgrade.A ? 3 : 2);
            }
            return bTimeAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AVariableHint() {
                status = (Status) Manifest.Statuses["BulletTime"].Id!
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, this.GetBTimeAmt(s)),
                xHint = upgrade == Upgrade.A ? 3 : 2,
            });
            if (upgrade != Upgrade.B) {
                actions.Add(new AStatus() {
                    status = (Status) Manifest.Statuses["BulletTime"].Id!,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                });
                actions.Add(new AUnfreeze() {
                    omitIncoming = false,
                });
            }
            return actions;
        }

        public override string Name() => "Ravages of Time";
    }
}
