using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LightningStrikes : Card {

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 0 : 1,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["LightningStrikesCardSpriteFlip"] : Manifest.Sprites["LightningStrikesCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AOtherPlayedHint() {
                amount = 1
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AMove() {
                    dir = -1,
                    targetPlayer = true,
                });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
                moveEnemy = upgrade == Upgrade.B ? 1 : 0
            });
            if (upgrade != Upgrade.B)
                actions.Add(new AMove() {
                    dir = 2,
                    targetPlayer = true,
                });
            else
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    fast = true,
                    moveEnemy = -2
                });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
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


        public override string Name() => "Lightning Strikes";
    }
}
