using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CarveReality : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
                buoyant = upgrade == Upgrade.A,
                exhaust = true,
            };
        }

        public int costIncrease = 0;
        public bool wasPlayed = false;

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status)Manifest.Statuses?["CarveReality"].Id!,
                statusAmount = 1,
                dialogueSelector = ".mezz_carveReality",
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = (Status) Manifest.Statuses?["BulletTime"].Id!,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
            });
            return actions;
        }

        public override string Name() => "Carve Reality";
    }
}
