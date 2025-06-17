using System;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MagmaInjection : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = true,
            };
        }
        private int GetHeatAmt(State s) {
            if (s.route is Combat) {
                return Math.Max(0, s.ship.Get(Status.heat) - 2);
            } else
                return 0;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B) {
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = -2,
                    mode = AStatusMode.Add,
                    targetPlayer = true
                });
                actions.Add(new AVariableHint() {
                    status = Status.heat,
                });
                actions.Add(new AStatus() {
                    status = Status.corrode,
                    statusAmount = GetHeatAmt(s),
                    xHint = 1,
                    targetPlayer = false
                });
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true
                });
            } else {
                if (upgrade == Upgrade.A)
                    actions.Add(new AAttack() {
                        damage = GetDmg(s, 1),
                        fast = true,
                    });
                else {
                    if (Manifest.hasKokoro)
                        actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                        Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                            Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                            amount: 1
                        ), new AAttack() {
                            damage = GetDmg(s, 1),
                            fast = true,
                        }).AsCardAction);
                    else
                        actions.Add(new StatCostAttack() {
                            action = new AAttack() {
                                damage = GetDmg(s, 1),
                                fast = true,
                            },
                            statusReq = Status.heat,
                            statusCost = 1,
                            cumulative = 0,
                            first = true,
                        });
                }
                if (Manifest.hasKokoro) {
                    for (int i = 0; i < 3; i++) {
                        actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                        Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                            Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                            amount: 1
                        ), new AAttack() {
                            status = i == 0 ? null : Status.corrode,
                            targetPlayer = false,
                            statusAmount = i == 0 ? 0 : 1,
                            damage = GetDmg(s, 1),
                            fast = true,
                        }).AsCardAction);
                    }
                } else {
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            damage = GetDmg(s, 1),
                            fast = true,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = upgrade == Upgrade.A ? 0 : 1,
                        first = upgrade == Upgrade.None,
                    });
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            damage = GetDmg(s, 1),
                            status = Status.corrode,
                            targetPlayer = false,
                            statusAmount = 1,
                            fast = true,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = upgrade == Upgrade.A ? 1 : 2,
                        first = false,
                        damage = GetDmg(s, 1),
                        status = Status.corrode,
                        targetPlayer = false,
                        statusAmount = 1,
                    });
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            status = Status.corrode,
                            targetPlayer = false,
                            statusAmount = 1,
                            damage = GetDmg(s, 1),
                            fast = true,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = upgrade == Upgrade.A ? 2 : 3,
                        first = false,
                        damage = GetDmg(s, 1),
                        status = Status.corrode,
                        targetPlayer = false,
                        statusAmount = 1,
                    });
                }
            }
            return actions;
        }

        public override string Name() => "Searing Rupture";
    }
}
