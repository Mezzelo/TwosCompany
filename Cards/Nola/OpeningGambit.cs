using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, unreleased = true, dontOffer = true)]
    public class OpeningGambit : Card, ITurnIncreaseCard {

        public int increasePerTurn = 1;
        public int costIncrease = 0;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = true,
                buoyant = true,
                exhaust = true
            };
        }
        int ITurnIncreaseCard.increasePerTurn { get => increasePerTurn; set => increasePerTurn = value; }
        int ITurnIncreaseCard.costIncrease { get => costIncrease; set => costIncrease = value; }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ATurnIncreaseHint() {
                amount = 1
            });
            int net = this.discount - (upgrade == Upgrade.A ? 3 : 2);
            actions.Add(new AEnergy() {
                changeAmount = upgrade == Upgrade.A ? 3 : (upgrade == Upgrade.B ? 1 : 2),
                dialogueSelector = upgrade != Upgrade.B && net > -1 ? ".mezz_openingGambit" : null
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.energyNextTurn,
                    statusAmount = 2,
                    targetPlayer = true
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


        public override string Name() => "Opening Gambit";
    }
}
