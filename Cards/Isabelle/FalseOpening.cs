using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FalseOpening : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 2,
                exhaust = upgrade == Upgrade.B
            };
        }
        private int GetShieldAmt(State s) {
            int shieldAmt = 0;
            if (s.route is Combat)
                shieldAmt = s.ship.Get(Status.shield);
            return shieldAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add((CardAction)new AVariableHint() {
                status = Status.shield
            });
            if (upgrade != Upgrade.B) {
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = GetShieldAmt(s),
                    targetPlayer = true,
                    xHint = 1,
                });
                if (upgrade == Upgrade.A)
                    actions.Add(new AStatus() {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true
                    });
            }
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 0,
                mode = AStatusMode.Set,
                targetPlayer = true
            });
            ExternalStatus falseStatus = Manifest.Statuses?["FalseOpening" + (upgrade == Upgrade.B ? "B" : "")] ?? throw new Exception("status missing: falseopening");
            actions.Add(new AStatus() {
                status = falseStatus.Id != null ? (Status)falseStatus.Id : Status.overdrive,
                statusAmount = 1,
                targetPlayer = true
            });
            return actions;
        }

        public override string Name() => "False Opening";
    }
}
