using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Postpone : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 2,
                exhaust = upgrade == Upgrade.B,
                recycle = upgrade != Upgrade.B,
                retain = upgrade == Upgrade.B,
            };
        }

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade != Upgrade.B)
                actions.Add(new ACostIncreasePlayedHint() {
                    amount = 1,
                });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status) Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                mode = upgrade == Upgrade.B ? AStatusMode.Add : AStatusMode.Set,
                dialogueSelector = ".mezz_postpone",
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.droneShift,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new AStatus() {
                    targetPlayer = true,
                    status = Status.overdrive,
                    statusAmount = 1
                });
            }

            return actions;
        }

        public override void OnExitCombat(State s, Combat c) {
            wasPlayed = false;
            costIncrease = 0;
        }

        public override void OnDraw(State s, Combat c) {
            if (upgrade != Upgrade.B && wasPlayed) {
                wasPlayed = false;
                this.discount += costIncrease;
            }
        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (upgrade != Upgrade.B) {
                wasPlayed = true;
                costIncrease++;
            }
        }

        public override void OnDiscard(State s, Combat c) {
            if (upgrade != Upgrade.B && !wasPlayed && costIncrease != 0) {
                this.discount -= costIncrease;
                costIncrease = 0;
            }
        }

        public override string Name() => "Postpone";
    }
}
