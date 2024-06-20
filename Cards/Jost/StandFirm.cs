using CobaltCoreModding.Definitions.ExternalItems;
using System.Collections.Generic;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class StandFirm : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
                retain = upgrade != Upgrade.B,
                floppable = upgrade == Upgrade.A,
                exhaust = upgrade == Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites[upgrade != Upgrade.A ? "JostDefaultCardSpriteUnsided" : ("StandFirmCardSprite" + (this.flipped ? "Flip" : ""))].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensiveStance");
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = (Status) defensiveStance.Id!,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            ExternalStatus standFirm = Manifest.Statuses?["StandFirm"] ?? throw new Exception("status missing: standfirm");
            actions.Add(new AStatus() {
                status = (Status) standFirm.Id!,
                statusAmount = 1,
                targetPlayer = true,
                disabled = upgrade == Upgrade.A && flipped,
            });

            if (upgrade == Upgrade.A) {
                actions.Add(new ADummyTooltip() {
                    action = new AStatus() {
                        status = (Status)Manifest.Statuses?[!flipped ? "Superposition" : "StandFirm"].Id!,
                        statusAmount = 1,
                        targetPlayer = true,
                        disabled = !flipped,
                    }
                });

                actions.Add(new AStatus() {
                    status = (Status) Manifest.Statuses?["Superposition"].Id!,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = !flipped,
                });
                actions.Add(new AExhaustSelf() {
                    uuid = this.uuid,
                    omitFromTooltips = false,
                    disabled = !flipped,
                });
            }

            return actions;
        }

        public override string Name() => "Stand Firm";
    }
}
