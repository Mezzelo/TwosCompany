namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, unreleased = true, dontOffer = true)]
    public class Grapple : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["GrappleCardSpriteFlip"] : Manifest.Sprites["GrappleCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 0),
                status = Status.lockdown,
                statusAmount = 1,
                piercing = upgrade == Upgrade.B
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true
                });
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.A ? 1 : 2,
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Grapple";
    }
}
