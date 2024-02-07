using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HyperspaceStorm : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            ExternalStatus hyperspaceStorm = Manifest.Statuses?[
                "HyperspaceStorm" + (upgrade == Upgrade.A ? "A" : (upgrade == Upgrade.B ? "B" : ""))
                ] ?? throw new Exception("status missing: hyperspaceStorm");
            actions.Add(new AStatus() {
                status = (Status)hyperspaceStorm.Id!,
                statusAmount = 1,
                targetPlayer = true,
                dialogueSelector = ".mezz_hyperspaceStorm",
            });
            actions.Add(new AAddCard() {
                card = new HyperspaceWind() { upgrade = this.upgrade },
                destination = CardDestination.Hand,
                amount = 1,
            });
            return actions;
        }

        public override string Name() => "Hyperspace Storm";
    }
}
