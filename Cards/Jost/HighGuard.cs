using CobaltCoreModding.Definitions.ExternalItems;
using System;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class HighGuard : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade != Upgrade.None ? "" : "Down1") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?["FalseOpening"].Id!,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());


            if (Manifest.hasKokoro) {
                CardAction hbAction = Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                    Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                        Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.shield),
                        amount:upgrade == Upgrade.B ? 1 : 2
                    ), new AStatus() {
                        status = Status.overdrive,
                        statusAmount = upgrade == Upgrade.B ? 1 : 2,
                        targetPlayer = true,
                    }
                ).AsCardAction;
                hbAction.disabled = Stance.Get(s) < 2;
                actions.Add(hbAction);
            } else
                actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    statusAmount = upgrade == Upgrade.B ? 1 : 2,
                    targetPlayer = true,
                },
                statusReq = Status.shield,
                statusCost = upgrade == Upgrade.B ? 1 : 2,
                first = true,
                disabled = Stance.Get(s) < 2,
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) < 2,
                });
            else if (upgrade == Upgrade.B)
                if (Manifest.hasKokoro) {
                    CardAction hbAction = Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                        Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                            Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.shield),
                            amount: 2
                        ), new AStatus() {
                            status = Status.overdrive,
                            statusAmount = 2,
                            targetPlayer = true,
                        }
                    ).AsCardAction;
                    hbAction.disabled = Stance.Get(s) < 2;
                    actions.Add(hbAction);
                } else
                    actions.Add(new StatCostAction() {
                        action = new AStatus() {
                            status = Status.overdrive,
                            statusAmount = 2,
                            targetPlayer = true,
                        },
                        statusReq = Status.shield,
                        statusCost = 2,
                        first = false,
                        cumulative = 1,
                        disabled = Stance.Get(s) < 2,
                    });

            return actions;
        }

        public override string Name() => "High Guard";
    }
}
