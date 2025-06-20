﻿using System;
using TwosCompany.Actions;
using static System.Collections.Specialized.BitVector32;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class MoltenShot : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
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

            int boostMod = 0;
            if (s.route is Combat)
                boostMod = s.ship.Get(Status.boost);

            if (Manifest.hasKokoro)
                actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                    Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                    amount: 1
                ), new AStatus() {
                    status = Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                }).AsCardAction);
            else
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
                damage = GetDmg(s, GetHeatAmt(s) > 0 ? 1 + boostMod : 0),
                fast = true,
            });
            if (Manifest.hasKokoro)
                actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                    Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                    amount: 1
                ), new AAttack() {
                    damage = GetDmg(s, GetHeatAmt(s) > 0 ? 2 + boostMod : 1),
                    fast = true,
                }).AsCardAction);
            else
                actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    damage = GetDmg(s, GetHeatAmt(s) > 0 ? 2 + boostMod : 1),
                    fast = true,
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = 1,
                timer = -0.5,
            });
            if (upgrade == Upgrade.B) {
                if (Manifest.hasKokoro)
                    actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                    Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                        Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                        amount: 1
                    ), new AAttack() {
                        damage = GetDmg(s, GetHeatAmt(s) > 0 ? 2 + boostMod : 1),
                        fast = true,
                    }).AsCardAction);
                else
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            damage = GetDmg(s, GetHeatAmt(s) > 0 ? 2 + boostMod : 1),
                            fast = true,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = 2,
                        timer = -0.5,
                    });
            }
            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    targetPlayer = true,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                });
            return actions;
        }

        public override string Name() => "Molten Shot";
    }
}
