using System.ComponentModel.Design;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BurningIn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                flippable = upgrade != Upgrade.None,
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

            actions.Add(new AVariableHint() {
                status = Status.heat
            });
            actions.Add(new AMove() {
                dir = this.GetHeatAmt(s) * (upgrade == Upgrade.B ? 2 : 1),
                xHint = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, this.GetHeatAmt(s)),
                xHint = 1,
                dialogueSelector = this.GetHeatAmt(s) > 3 ? ".mezz_thermalBlast" : null
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, this.GetHeatAmt(s)),
                    xHint = 1,
                });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = -2,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Burning In";
    }
}
