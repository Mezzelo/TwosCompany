﻿using System;
using System.Text;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Imbue : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                exhaust = upgrade == Upgrade.B,
            };
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
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = upgrade != Upgrade.None ? Status.overdrive : Status.shield,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = 1,
            });
            actions.Add(new StatCostAction() {
                action = upgrade == Upgrade.B ? new AStatus() {
                    status = (Status) Manifest.Statuses?["Enflamed"].Id!,
                    targetPlayer = true,
                    statusAmount = 1,
                } : new AHurt() {
                    hurtAmount = 1,
                    targetPlayer = true,
                    hurtShieldsFirst = false,
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = 2,
            });
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.powerdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                    dialogueSelector = ".mezz_imbue",
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = 3,
            });
            return actions;
        }

        public override string Name() => "Imbue";
    }
}
