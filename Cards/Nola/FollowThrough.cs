using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FollowThrough : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                recycle = upgrade == Upgrade.B,
                retain = upgrade != Upgrade.B,
                floppable = upgrade == Upgrade.A,
                art = new Spr?((Spr)(Manifest.Sprites[
                    upgrade == Upgrade.A ? "NolaCardSpriteUp1" + (flipped ? "Flip" : "") : "FollowThroughCardSprite"].Id
                    ?? throw new Exception("missing flop art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status) Manifest.Statuses?["FollowUp"].Id!,
                statusAmount = 1,
                disabled = upgrade == Upgrade.A && flipped,
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new ADummyAction() {
                });
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["FollowUp"].Id!,
                    statusAmount = 1,
                    disabled = !flipped,
                });
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["Onslaught"].Id!,
                    statusAmount = 1,
                    disabled = !flipped,
                });
            }
            return actions;
        }


        public override void OnDraw(State state, Combat c) {
            flipped = false;
        }

        public override string Name() => "Follow Through";
    }
}
