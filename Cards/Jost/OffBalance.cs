using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, dontOffer = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class OffBalance : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
                temporary = true,
                exhaust = true,
                retain = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            actions.Add(new AStatus() {
                status = (Status) defensiveStance.Id!,
                statusAmount = 1,
                mode = upgrade == Upgrade.A ? AStatusMode.Set : AStatusMode.Add,
                targetPlayer = true,
                dialogueSelector = Stance.Get(s) == 0 ? ".mezz_offBalance" : null,
            });
            if (upgrade == Upgrade.A) {
                ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
                actions.Add(new AStatus() {
                    status = (Status) offensiveStance.Id!,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = true,
                });
            }
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 2,
                mode = AStatusMode.Add,
                targetPlayer = true,
            });
            if (upgrade != Upgrade.B)
                actions.Add(new ADiscard() {
                    count = 2,
                });


            return actions;
        }

        public override string Name() => "Off Balance";
    }
}
