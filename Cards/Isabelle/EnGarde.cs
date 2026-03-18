using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EnGarde : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 2,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["EnGardeCardSpriteFlip"] : Manifest.Sprites["EnGardeCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses["TempStrafe"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
            });
            actions.Add(new AStatus() {
                status = Status.libra,
                statusAmount = 1,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.engineStall,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            
            return actions;
        }
        public override string Name() => "En Garde";
    }
}
