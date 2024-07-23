using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RaisedPalm : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                retain = true,
            };
        }
        private int GetBTimeAmt(State s) {
            int bTimeAmt = 0;
            if (s.route is Combat) {
                bTimeAmt = s.ship.Get((Status)Manifest.Statuses["BulletTime"].Id!);
            }
            return bTimeAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AVariableHint() {
                status = (Status)Manifest.Statuses["BulletTime"].Id!
            });
            actions.Add(new AHurtSilent() {
                hurtAmount = GetBTimeAmt(s),
                targetPlayer = true,
                hurtShieldsFirst = false,
                xHint = 1,
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = 2,
                dialogueSelector = ".mezz_raisedPalm",
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.droneShift,
                statusAmount = upgrade == Upgrade.A ? 3 : 2
            });
            if (upgrade == Upgrade.B) {
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = Status.overdrive,
                    statusAmount = 1
                });
            }

            return actions;
        }

        public override string Name() => "Raised Palm";
    }
}
