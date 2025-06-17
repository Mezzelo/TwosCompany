using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Haze : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat)
                heatAmt = s.ship.Get(Status.heat) + (upgrade == Upgrade.B ? 1 + s.ship.Get(Status.boost) : 0);
            return heatAmt;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            actions.Add(new AVariableHint() {
                status = Status.heat
            });
            actions.Add(new AMove() {
                dir = this.GetHeatAmt(s),
                xHint = 1,
                isRandom = true,
                targetPlayer = true
            });
            if (upgrade == Upgrade.None) {
                if (Manifest.hasKokoro)
                    actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                    Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                        Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.evade),
                        amount: 1
                    ), new AStatus() {
                        status = Status.evade,
                        targetPlayer = true,
                        statusAmount = 1,
                    }).AsCardAction);
                else
                    actions.Add(new StatCostAction() {
                        action = new AStatus() {
                            status = Status.evade,
                            targetPlayer = true,
                            statusAmount = 1,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        first = true
                    });
            }
            actions.Add(new AStatus() {
                status = upgrade != Upgrade.None ? Status.evade : Status.heat,
                statusAmount = 1,
                targetPlayer = true,
            });

            return actions;
        }

        public override string Name() => "Haze";
    }
}
