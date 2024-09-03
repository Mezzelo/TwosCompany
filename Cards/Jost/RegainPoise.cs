using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RegainPoise : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = upgrade != Upgrade.B,
                floppable = upgrade == Upgrade.A,
                art = new Spr?((Spr)(Manifest.Sprites[upgrade != Upgrade.A ? "JostDefaultCardSpriteUnsided" : ("RegainPoiseCardSprite" + (this.flipped ? "Flip" : ""))].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?["DefensiveStance"].Id!,
                statusAmount = 1,
                mode = upgrade == Upgrade.B ? AStatusMode.Add : AStatusMode.Set,
                targetPlayer = true,
                dialogueSelector = Stance.Get(s) == 0 ? ".mezz_offBalance" : null,
                disabled = flipped && upgrade == Upgrade.A,
            });
            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?["OffensiveStance"].Id!,
                statusAmount = 0,
                mode = AStatusMode.Set,
                targetPlayer = true,
                disabled = flipped && upgrade == Upgrade.A,
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new ADummyAction());
                actions.Add(new AStatus() {
                    status = (Status)Manifest.Statuses?["OffensiveStance"].Id!,
                    statusAmount = 1,
                    mode = upgrade == Upgrade.B ? AStatusMode.Add : AStatusMode.Set,
                    targetPlayer = true,
                    dialogueSelector = Stance.Get(s) == 0 ? ".mezz_offBalance" : null,
                    disabled = !flipped,
                });
                actions.Add(new AStatus() {
                    status = (Status)Manifest.Statuses?["DefensiveStance"].Id!,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                    disabled = !flipped,
                });
            }

            return actions;
        }

        public override string Name() => "Regain Poise";
    }
}
