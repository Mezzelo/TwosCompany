using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Harry : Card, ITCNickelTraits {

        public int costIncrease = 0;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
                infinite = true,
            };
        }
        public string[] GetTraits()
            => new string[] { upgrade == Upgrade.B ? "AllIncreaseCombat" : "AllIncrease" };

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (!Manifest.hasNickel) {
                actions.Add(new AAllIncreaseHint() {
                    amount = 1,
                    isCombat = upgrade == Upgrade.B
                });
            }
            actions.Add(new AAttack() {
                damage = GetDmg(s, costIncrease + 1),
                fast = true,
            });
            actions.Add(new AMove() {
                dir = (upgrade == Upgrade.A ? 2 : 1) + costIncrease,
                targetPlayer = true,
                isRandom = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, costIncrease + 1),
                fast = true,
                dialogueSelector = costIncrease > 0 ? ".mezz_harry" : null,
            });
            actions.Add(new AMove() {
                dir = (upgrade == Upgrade.A ? 2 : 1) + costIncrease,
                targetPlayer = true,
                isRandom = true,
            });
            return actions;
        }


        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 0;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade != Upgrade.B) {
                this.discount -= costIncrease;
                costIncrease = 0;
            }
            
        }

        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease++;
            this.discount += costIncrease;
        }

        public override string Name() => "Harry";
    }
}
