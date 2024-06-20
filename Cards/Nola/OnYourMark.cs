using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class OnYourMark : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                buoyant = upgrade == Upgrade.B,
                floppable = upgrade == Upgrade.A,
                exhaust = true,
                art = new Spr?((Spr)(Manifest.Sprites[
                    upgrade == Upgrade.A ? "AdaptationCardSprite" + (flipped ? "Flip" : "") : "OnYourMarkCardSprite"].Id
                    ?? throw new Exception("missing flop art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["FollowUp"].Id!,
                statusAmount = 3,
                disabled = upgrade == Upgrade.A && flipped,
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["Onslaught"].Id!,
                statusAmount = 3,
                disabled = upgrade == Upgrade.A && flipped,
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new ADummyAction());

                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = (Status)Manifest.Statuses?["FollowUp"].Id!,
                    statusAmount = 3,
                    disabled = !flipped,
                });
                actions.Add(new ADrawCard() {
                    count = 3,
                    disabled = !flipped,
                });
            }
            return actions;
        }

        public override string Name() => "Take The Lead";
    }
}
