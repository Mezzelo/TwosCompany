using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, dontOffer = true)]
    public class Recover : Card {
        public bool forTooltip = false;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
                exhaust = true,
                flippable = true,
                retain = true,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["RecoverCardSpriteFlip"] : Manifest.Sprites["RecoverCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AOtherPlayedHint() {
                amount = 1,
                omitFromTooltips = forTooltip
            });
            actions.Add(new AMove() {
                dir = 2,
                targetPlayer = true,
                isRandom = false,
                omitFromTooltips = forTooltip
            }) ;
            return actions;
        }
        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition) {
            this.discount += 1;
        }
        public override string Name() => "Recover";
    }
}
