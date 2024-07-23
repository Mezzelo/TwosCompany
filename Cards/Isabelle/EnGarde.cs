using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class EnGarde : Card, IOtherAttackIncreaseCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                buoyant = upgrade == Upgrade.A,
                exhaust = true,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["EnGardeCardSpriteFlip"] : Manifest.Sprites["EnGardeCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public int costIncrease = 0;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ACostIncreaseAttackHint() {
                amount = 1,
            });
            actions.Add(new AStatus() {
                status = Status.autododgeLeft,
                statusAmount = 1,
                targetPlayer = false
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = 2,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = 2
                });
            return actions;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease = 0;
        }
        public override void OnExitCombat(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }
        public void OtherAttackDiscount(State s) {
            costIncrease++;
            this.discount++;
        }
        public override void OnDiscard(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }

        public override string Name() => "En Garde";
    }
}
