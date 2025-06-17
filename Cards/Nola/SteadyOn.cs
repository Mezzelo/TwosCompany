using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SteadyOn : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    targetPlayer = true,
                    statusAmount = -1,
                });
            if (Manifest.hasKokoro) {
                for (int i = 0; i < 3; i++)
                    actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                        Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                            Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.evade),
                        amount: 1), new AStatus() {
                            status = upgrade == Upgrade.B && i == 2 ? Status.powerdrive : Status.overdrive,
                            targetPlayer = true,
                            statusAmount = 1,
                        }
                    ).AsCardAction);
                return actions;
            }
            
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.evade,
                statusCost = 1,
                cumulative = upgrade == Upgrade.A ? 0 : 1,
            });
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                    dialogueSelector = ".mezz_steadyOn"
                },
                statusReq = Status.evade,
                statusCost = 1,
                cumulative = upgrade == Upgrade.A ? 1 : 2
            });
            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                },
                statusReq = Status.evade,
                statusCost = 1,
                cumulative = upgrade == Upgrade.A ? 2 : 3
            });
            return actions;
        }

        public override string Name() => "Steady On";
    }
}
