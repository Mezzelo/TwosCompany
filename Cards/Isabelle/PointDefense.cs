using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class PointDefense : Card {

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                infinite = true,
                retain = costIncrease == 0,
                flippable = upgrade == Upgrade.A
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            /*
            actions.Add(new ACostDecreasePlayedHint() {
                amount = 1
            }); */
            APDMove moveAction = new APDMove() {
                selectedCard = this,
                isRight = !flipped
            };
            bool disableAttacks = moveAction.CalculateMove(s, c, out _) == null;
            actions.Add(moveAction);
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                disabled = (s.route is Combat) && disableAttacks
            });
            if (upgrade == Upgrade.B) {
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    disabled = (s.route is Combat) && disableAttacks
                });
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 2),
                    disabled = (s.route is Combat) && disableAttacks
                });
            }
            return actions;
        }
        

        public override void OnExitCombat(State s, Combat c) {
            wasPlayed = false;
            costIncrease = 0;
        }

        public override void OnDraw(State s, Combat c) {

        }

        public override void AfterWasPlayed(State state, Combat c) {
            wasPlayed = true;
            costIncrease++;
            // this.discount -= costIncrease;
        }

        public override void OnDiscard(State s, Combat c) {
            if (!wasPlayed && costIncrease != 0) {
                // this.discount += costIncrease;
                costIncrease = 0;
            }
            else if (wasPlayed)
                wasPlayed = false;
        }

        public override string Name() => "Point Defense";
    }
}
