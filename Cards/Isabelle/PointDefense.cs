using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class PointDefense : Card {

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
                infinite = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new APDMove() {
                isRight = true
            }) ;
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                });
            return actions;
        }


        public override void OnExitCombat(State s, Combat c) {
            wasPlayed = false;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade == Upgrade.B && wasPlayed) {
                wasPlayed = false;
                this.discount -= costIncrease;
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
                this.discount += costIncrease;
                costIncrease = 0;
            }
        }

        public override string Name() => "Point Defense";
    }
}
