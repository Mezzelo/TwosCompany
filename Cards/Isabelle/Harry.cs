using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Harry : Card {

        public int costIncrease = 1;
        // public bool wasPlayed = false;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
                infinite = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAllIncreaseHint() {
                amount = 1,
                isCombat = upgrade == Upgrade.B
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, costIncrease),
                fast = true,
            });
            actions.Add(new AMove() {
                dir = (upgrade == Upgrade.None ? 1 : 0) + costIncrease,
                targetPlayer = true,
                isRandom = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, costIncrease),
                fast = true,
                dialogueSelector = costIncrease > 1 ? ".mezz_harry" : null,
            });
            actions.Add(new AMove() {
                dir = (upgrade == Upgrade.None ? 1 : 0) + costIncrease,
                targetPlayer = true,
                isRandom = true,
            });
            return actions;
        }


        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 1;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade != Upgrade.B) {
                this.discount -= costIncrease - 1;
                costIncrease = 1;
            }
            
        }

        public override void AfterWasPlayed(State state, Combat c) {
            this.discount += costIncrease;
            costIncrease++;
            // wasPlayed = true;
        }

        public override string Name() => "Harry";
    }
}
