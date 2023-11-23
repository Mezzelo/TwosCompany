using System;
using TwosCompany.Actions;
using static System.Collections.Specialized.BitVector32;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MoltenShot : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
            };
        }
        private int GetHeatAmt(State s) {
            int heatAmt = 0;
            if (s.route is Combat)
                heatAmt = s.ship.Get(Status.heat);
            return heatAmt;
        }
        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.heat,
                statusCost = 1,
                first = true
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, GetHeatAmt(s) > 0 ? 1 : 0),
                fast = true,
            });
            actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    damage = GetDmg(s, GetHeatAmt(s) > 0 ? 2 : 1),
                    fast = true,
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = 1,
                timer = -0.5,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new StatCostAttack() {
                    action = new AAttack() {
                        damage = GetDmg(s, GetHeatAmt(s) > 0 ? 2 : 1),
                        fast = true,
                    },
                    statusReq = Status.heat,
                    statusCost = 1,
                    cumulative = 2,
                    timer = -0.5,
                });
            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    targetPlayer = true,
                    statusAmount = upgrade == Upgrade.B ? 3 : 1,
                });
            return actions;
        }

        public override string Name() => "Molten Shot";
    }
}
