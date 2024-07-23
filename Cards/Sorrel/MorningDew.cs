using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MorningDew : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = upgrade == Upgrade.A,
                recycle = upgrade == Upgrade.B,
            };
        }
        private int GetBTimeAmt(State s) {
            int bTimeAmt = 0;
            if (s.route is Combat) {
                bTimeAmt = s.ship.Get((Status)Manifest.Statuses["BulletTime"].Id!) * 3;
            }
            return bTimeAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AVariableHint() {
                status = (Status)Manifest.Statuses["BulletTime"].Id!
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.tempShield,
                statusAmount = GetBTimeAmt(s),
                xHint = 3,
                dialogueSelector = GetBTimeAmt(s) > 0 ? ".mezz_morningDew" : null,
            });
            actions.Add(new AUnfreeze() {
                omitIncoming = false,
            });
            return actions;
        }
        public override string Name() => "Morning Dew";
    }
}
