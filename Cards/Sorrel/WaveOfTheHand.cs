using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Sorrel {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class WaveOfTheHand : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = upgrade != Upgrade.None,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new AForceAttack() {
                    queueAfter = 2,
                });
            actions.Add(new ADroneMove() {
                dir = 4,
            });
            actions.Add(new AUnfreeze() {
                omitIncoming = false,
                dialogueSelector = ".mezz_waveOfTheHand",
            });
            return actions;
        }

        public override string Name() => "Wave of the Hand";
    }
}
