using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class TimeHealsAll : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 0 : 1,
                exhaust = true,
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
                status = (Status) Manifest.Statuses?["BulletTime"].Id!
            });
            actions.Add(new AHeal() {
                targetPlayer = true,
                healAmount = GetBTimeAmt(s),
                xHint = 1,
                canRunAfterKill = true,
                dialogueSelector = GetBTimeAmt(s) > 0 ? ".mezz_timeHealsAll" : null,
            });
            if (upgrade != Upgrade.B) {
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                });
                actions.Add(new AUnfreeze() {
                    omitIncoming = false,
                });
            }
            return actions;
        }

        public override string Name() => "Time Heals All";
    }
}
