using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class HyperspaceWind : Card, ITurnIncreaseCard {

        public int costIncrease = 0;
        public bool wasPlayed = false;
        public int increasePerTurn = 1;

        public override CardData GetData(State state) {

            return new CardData() {
                cost = 0,
                retain = upgrade != Upgrade.B,
                flippable = true,
                temporary = true,
                exhaust = true,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["HyperspaceWindCardSpriteFlip"] : Manifest.Sprites["HyperspaceWindCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }
        int ITurnIncreaseCard.increasePerTurn { get => upgrade != Upgrade.A ? increasePerTurn : 0; set => increasePerTurn = value; }
        int ITurnIncreaseCard.costIncrease { get => costIncrease; set => costIncrease = value; }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade == Upgrade.None)
                actions.Add(new ATurnIncreaseHint() {
                    amount = 1
                });
            actions.Add(new ADroneMove() {
                dir = 1,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 0),
                });
            return actions;
        }

        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease = 0;
        }
        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 0;
        }
        public override void OnDiscard(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }

        public override string Name() => "Hyperspace Wind";
    }
}
