using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ConduitCard : Card {

        public int costIncrease = 0;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
                infinite = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade == Upgrade.B)
                actions.Add(new AAllIncreaseHint() {
                    amount = 1,
                    isCombat = upgrade != Upgrade.B,
                });
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.normal,
                    bubbleShield = upgrade == Upgrade.A,
                },
            });
            actions.Add(new AStatus() {
                status = Status.droneShift,
                statusAmount = upgrade == Upgrade.B ? costIncrease : 1,
                targetPlayer = true,
            });
            return actions;
        }
        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 0;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade == Upgrade.B) {
                this.discount -= costIncrease;
                costIncrease = 0;
            }

        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (upgrade == Upgrade.B) {
                costIncrease += upgrade == Upgrade.B && Stance.Get(state) == 3 ? 2 : 1;
                this.discount += costIncrease;
            }
        }

        public override string Name() => "Conduit";
    }
}
