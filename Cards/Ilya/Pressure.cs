using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Pressure : Card, ITCNickelTraits {

        public int costIncrease = 0;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                infinite = upgrade == Upgrade.B,
            };
        }
        public string[] GetTraits()
            => upgrade == Upgrade.B ? new string[] { "EnergyPerPlay" } : new string[] { };

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                if (!Manifest.hasNickel)
                    actions.Add(new ACostIncreasePlayedHint() {
                        amount = 1,
                    });
            else {
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
            }

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 1,
                targetPlayer = true,
            });
            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 3,
                targetPlayer = false,
            });
            actions.Add(new ADrawCard() {
                count = upgrade != Upgrade.None ? 2 : 1
            });
            return actions;
        }


        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 0;
        }
        public override void OnDraw(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (upgrade == Upgrade.B) {
                costIncrease++;
                this.discount += costIncrease;
            }
        }

        public override string Name() => "Pressure";
    }
}
