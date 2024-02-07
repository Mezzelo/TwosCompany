using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class MoveAsOne : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 3,
                retain = upgrade == Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public int costIncrease = 0;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ACostDecreaseAttackHint() {
                amount = 1,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.A ? 4 : 3,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new ADummyAction());
            actions.Add(new AOtherPlayedHint() {
                amount = 1,
                perma = true,
                disabled = Stance.Get(s) < 2,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 4),
                stunEnemy = true,
                piercing = upgrade == Upgrade.A,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            costIncrease = 0;
        }
        public override void OnExitCombat(State s, Combat c) {
            this.discount -= costIncrease;
            costIncrease = 0;
        }
        public override void OnOtherCardPlayedWhileThisWasInHand(State s, Combat c, int handPosition) {
            if (Stance.Get(s) > 1) {
                this.discount += 1;
                costIncrease++;
            }
        }

        public void OtherAttackDiscount() {
            costIncrease-= 1;
            this.discount-= 1;
        }

        public override string Name() => "MoveAsOne";
    }
}
