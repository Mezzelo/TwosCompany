using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class OpeningGambit : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = true,
                buoyant = true,
                exhaust = true
            };
        }

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AOtherPlayedHint() {
                amount = 1
            });
            actions.Add(new AEnergy() {
                changeAmount = upgrade == Upgrade.A ? 4 : 2
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
            // this.discount -= costIncrease;
            costIncrease = 0;
            wasPlayed = false;
        }

        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition) {
            this.discount += 1;
            costIncrease++;
        }
        public override void OnDiscard(State s, Combat c) {
            if (wasPlayed)
                wasPlayed = false;
            else
                this.discount -= costIncrease;
            costIncrease = 0;
        }


        public override string Name() => "Opening Gambit";
    }
}
