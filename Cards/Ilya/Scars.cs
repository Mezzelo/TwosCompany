﻿namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Scars : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                exhaust = upgrade != Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AHurt() {
                hurtAmount = 2,
                targetPlayer = true,
                hurtShieldsFirst = false
            });
            actions.Add(new AHullMax() {
                amount = 1,
                targetPlayer = true
            });
            actions.Add(new AEnergy() {
                changeAmount = upgrade == Upgrade.A ? 2 : 1
            });
            actions.Add(new ADrawCard() {
                count = 2
            });
            return actions;
        }

        public override string Name() => "Scars";
    }
}
