﻿using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Onslaught : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
                retain = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus onslaughtStatus = Manifest.Statuses?["Onslaught"] ?? throw new Exception("status missing: onslaught");
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = onslaughtStatus.Id != null ? (Status)onslaughtStatus.Id : Status.drawNextTurn,
                statusAmount = 3
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = 2
                });
            return actions;
        }

        public override string Name() => "Onslaught";
    }
}
