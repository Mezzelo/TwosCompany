using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Bind : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
            };
        }

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
            actions.Add(new AOtherPlayedHint() {
                amount = 1,
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.A ? Status.shield : Status.tempShield,
                statusAmount = 2,
                targetPlayer = true,
                dialogueSelector = ".mezz_bind"
            });
            actions.Add(new AStatus() {
                status = Status.engineStall,
                statusAmount = 2,
                targetPlayer = true
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = strafeStatus.Id != null ? (Status) strafeStatus.Id : Status.strafe,
                    statusAmount = 1,
                    targetPlayer = true,
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

        public override string Name() => "Bind";
    }
}
