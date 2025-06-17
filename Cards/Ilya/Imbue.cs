using System;
using System.Text;
using TwosCompany.Actions;
using static System.Collections.Specialized.BitVector32;

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
            if (Manifest.hasKokoro)
                actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                    Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                    amount: 1
                ), new AStatus() {
                    status = upgrade != Upgrade.None ? Status.overdrive : Status.shield,
                    targetPlayer = true,
                    statusAmount = 1,
                }).AsCardAction);
            else
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
            if (Manifest.hasKokoro)
                actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                    Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                    amount: 1
                ), upgrade == Upgrade.B ? new AStatus() {
                    status = (Status)Manifest.Statuses?["Enflamed"].Id!,
                    targetPlayer = true,
                    statusAmount = 1,
                } : new AHurt() {
                    hurtAmount = 1,
                    targetPlayer = true,
                    hurtShieldsFirst = false,
                }).AsCardAction);
            else
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
            if (Manifest.hasKokoro)
                actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                    Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                    amount: 1
                ), new AStatus() {
                    status = Status.powerdrive,
                    targetPlayer = true,
                    statusAmount = 1,
                    dialogueSelector = ".mezz_imbue",
                }).AsCardAction);
            else
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
