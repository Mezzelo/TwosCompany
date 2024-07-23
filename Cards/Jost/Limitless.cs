using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Limitless : Card {

        public bool breatheInRetain = false;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = true,
                singleUse = upgrade != Upgrade.B,
                exhaust = upgrade == Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses?["Superposition"].Id!,
                statusAmount = upgrade == Upgrade.B ? 1 : (upgrade == Upgrade.A ? 3 : 2),
                targetPlayer = true,
            });
            actions.Add(new AAddCard() {
                amount = upgrade == Upgrade.A ? 3 : 2,
                card = new Heartbeat() { upgrade = Upgrade.None, temporaryOverride = upgrade == Upgrade.B },
                destination = CardDestination.Hand,
                showCardTraitTooltips = false,
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new AStatus() {
                    status = Status.shield,
                    statusAmount = 4,
                    targetPlayer = true,
                });
                actions.Add(new AStatus() {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true,
                });
            }
            return actions;
        }

        public override string Name() => "Limitless";
    }
}
