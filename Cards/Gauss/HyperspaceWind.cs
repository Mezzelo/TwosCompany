using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class HyperspaceWind : Card {

        public int costIncrease = 0;
        public bool wasPlayed = false;
        public override CardData GetData(State state) {

            return new CardData() {
                cost = 0,
                retain = upgrade != Upgrade.None,
                flippable = upgrade == Upgrade.B,
                temporary = true,
                exhaust = true,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["HyperspaceWindCardSpriteFlip"] : Manifest.Sprites["HyperspaceWindCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade == Upgrade.B)
                actions.Add(new AOtherPlayedHint() {
                    amount = 1,
                });
            actions.Add(new ADroneMove() {
                dir = -1,
            });
            return actions;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease = 0;
        }
        public override void OnExitCombat(State s, Combat c) {
            // this.discount -= costIncrease;
            costIncrease = 0;
            wasPlayed = false;
        }

        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition) {
            if (upgrade == Upgrade.B) {
                this.discount += 1;
                costIncrease++;
            }
        }
        public override void OnDiscard(State s, Combat c) {
            if (wasPlayed)
                wasPlayed = false;
            else
                this.discount -= costIncrease;
            costIncrease = 0;
        }

        public override string Name() => "Hyperspace Wind";
    }
}
