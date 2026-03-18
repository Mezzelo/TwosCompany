using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HereAndNow : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = Status.tempPayback,
                statusAmount = 2,
                targetPlayer = true
            });
            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses?["FalseOpening"].Id!,
                statusAmount = upgrade == Upgrade.B ? 4 : 2,
                targetPlayer = true
            });
            actions.Add(new AForceAttack() {
                queueAfter = 0
            });

            return actions;
        }

        public override string Name() => "Here and Now";
    }
}
