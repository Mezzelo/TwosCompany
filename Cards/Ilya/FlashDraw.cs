using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FlashDraw : Card {

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                recycle = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();


            if (upgrade == Upgrade.B)
                actions.Add(new ACostIncreasePlayedHint() {
                    amount = 1,
                });

            actions.Add(new ADiscard());
            actions.Add(new ADrawCard() {
                count = 5
            });

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = upgrade == Upgrade.None ? 2 : 1,
                targetPlayer = true,
            });
            return actions;
        }


        public override void OnExitCombat(State s, Combat c) {
            wasPlayed = false;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade == Upgrade.B && wasPlayed) {
                wasPlayed = false;
                this.discount += costIncrease;
            }
        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (upgrade == Upgrade.B) {
                wasPlayed = true;
                costIncrease++;
            }
        }

        public override void OnDiscard(State s, Combat c) {
            if (upgrade == Upgrade.B && !wasPlayed && costIncrease != 0) {
                this.discount -= costIncrease;
                costIncrease = 0;
            }
        }

        public override string Name() => "Flash Draw";
    }
}
