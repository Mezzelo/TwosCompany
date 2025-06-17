using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class SuddenShift : Card, ITCNickelTraits {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                flippable = true,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["SuddenShiftCardSpriteFlip"] : Manifest.Sprites["SuddenShiftCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public int costIncrease = 0;
        public bool wasPlayed = false;
        public string[] GetTraits()
            => new string[] { "EnergyPerCard" };

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (!Manifest.hasNickel)
                actions.Add(new AOtherPlayedHint() {
                    amount = upgrade == Upgrade.B ? 2 : 1
                });
            actions.Add(new AMove() {
                dir = upgrade == Upgrade.A ? 1 : 2,
                targetPlayer = true,
                // isRandom = true
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AEnergy() {
                    changeAmount = 1
                });
            else if (upgrade == Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.evade,
                    statusAmount = 1,
                    targetPlayer = true
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
            this.discount += upgrade == Upgrade.B ? 2 : 1;
            costIncrease+= upgrade == Upgrade.B ? 2 : 1;
        }
        public override void OnDiscard(State s, Combat c) {
            if (wasPlayed)
                wasPlayed = false;
            else
                this.discount -= costIncrease;
            costIncrease = 0;
        }

        public override string Name() => "Sudden Shift";
    }
}
