using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class FaceDownFate : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                exhaust = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                mode = upgrade == Upgrade.B ? AStatusMode.Add : AStatusMode.Set,
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.droneShift,
                statusAmount = upgrade == Upgrade.None ? 1 : 2,
                mode = AStatusMode.Add,
            });
            actions.Add(new AForceAttack() {
                dialogueSelector = ".mezz_faceDownFate",
            });
            return actions;
        }

        public override string Name() => "Face Down Fate";
    }
}
