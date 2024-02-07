using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class StandFirm : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = upgrade != Upgrade.B,
                floppable = upgrade == Upgrade.A,
                art = new Spr?((Spr)(Manifest.Sprites[upgrade != Upgrade.A ? "JostDefaultCardSpriteUnsided" : ("StandFirmCardSprite" + (this.flipped ? "Flip" : ""))].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensiveStance");
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensiveStance");
            if (upgrade == Upgrade.A)
                actions.Add(new StatCostAction() {
                    action = new AStatus() {
                        status = (Status) defensiveStance.Id!,
                        statusAmount = 1,
                        targetPlayer = true,
                    },
                    statusReq = (Status) offensiveStance.Id!,
                    statusCost = 1,
                    first = true,
                    disabled = flipped,
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new StatCostAction() {
                    action = new AStatus() {
                        status = (Status)defensiveStance.Id!,
                        statusAmount = 1,
                        targetPlayer = true,
                    },
                    statusReq = Status.shield,
                    statusCost = 4,
                    first = true,
                });

            ExternalStatus standFirm = Manifest.Statuses?["StandFirm"] ?? throw new Exception("status missing: standfirm");
            actions.Add(new AStatus() {
                status = (Status) standFirm.Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
                disabled = upgrade == Upgrade.A && flipped,
            });

            if (upgrade == Upgrade.A) {
                actions.Add(new ADummyAction());
                actions.Add(new StatCostAction() {
                    action = new AStatus() {
                        status = (Status) offensiveStance.Id!,
                        statusAmount = 1,
                        targetPlayer = true,
                    },
                    statusReq = (Status) defensiveStance.Id!,
                    statusCost = 1,
                    first = true,
                    disabled = !flipped,
                });

                actions.Add(new AStatus() {
                    status = (Status)standFirm.Id!,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true,
                    disabled = !flipped,
                });
            }

            return actions;
        }

        public override string Name() => "Stand Firm";
    }
}
