using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class OpeningGambit : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = Math.Max(0, GetRound(state) - roundDrawn),
                retain = true,
                buoyant = true,
                exhaust = true
            };
        }

        public int roundDrawn = 0;

        private int GetRound(State s) {
            int currentRound = 0;
            if (s.route is Combat)
                currentRound = ((Combat)s.route).turn;
            return currentRound;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ATurnIncreaseHint() {
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
        public override void OnExitCombat(State s, Combat c) {
            roundDrawn = 0;
        }
        public override void OnDraw(State s, Combat c) {
            roundDrawn = c.turn;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            roundDrawn = 0;
        }
        public override void OnDiscard(State s, Combat c) {
            roundDrawn = 0;
        }


        public override string Name() => "Opening Gambit";
    }
}
