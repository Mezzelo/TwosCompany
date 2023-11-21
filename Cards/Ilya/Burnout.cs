﻿namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Burnout : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
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

            actions.Add((CardAction)new AVariableHint() {
                status = new Status?(Status.heat)
            });
            actions.Add(new ADiscard() {
                count = GetHeatAmt(s),
                xHint = 1
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 0,
               mode = AStatusMode.Set,
                targetPlayer = true
            });

            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.heat,
                    statusAmount = 1,
                    targetPlayer = true
                });

            if (upgrade != Upgrade.None)
                actions.Add(new ADrawCard() {
                    count = upgrade == Upgrade.A ? 2 : 3,
                });

            return actions;
        }

        public override string Name() => "Burnout";
    }
}
