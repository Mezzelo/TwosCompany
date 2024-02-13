using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BattleTempo : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 1 : 2,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADummyAction());
            ExternalStatus battleTempo = Manifest.Statuses?["BattleTempo"] ?? throw new Exception("status missing: battleTempo");
            actions.Add(new AStatus() {
                status = (Status) battleTempo.Id!,
                statusAmount = 1,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.shield,
                    statusAmount = 3,
                    targetPlayer = true,
                });
            actions.Add(new ADummyTooltip() {
                action = new AAddCard() {
                    card = new Heartbeat() { temporaryOverride = true, exhaustOverride = true },
                    destination = CardDestination.Hand,
                }
            });

            return actions;
        }

        public override string Name() => "Battle Tempo";
    }
}
