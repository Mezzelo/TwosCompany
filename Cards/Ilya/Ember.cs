﻿using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, dontOffer = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Ember : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
                retain = true,
                exhaust = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
            });
            if (Manifest.hasKokoro)
                actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                    Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                    amount: 3
                ), new AHurt() {
                    hurtAmount = 1,
                    targetPlayer = true,
                    hurtShieldsFirst = false,
                }).AsCardAction);
            else
                actions.Add(new StatCostAction() {
                    action = new AHurt() {
                        hurtAmount = 1,
                        targetPlayer = true,
                        hurtShieldsFirst = false,
                    },
                    statusReq = Status.heat,
                    statusCost = 3,
                    first = true
                });
            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            return actions;
        }

        public override string Name() => "Ember";
    }
}
