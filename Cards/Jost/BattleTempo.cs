using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class BattleTempo : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                buoyant = upgrade == Upgrade.A,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>() { 
                new ADummyAction(),
                new AStatus() {
                    status = (Status) Manifest.Statuses?["BattleTempo"].Id!,
                    statusAmount = 1,
                    targetPlayer = true,
                }
            };
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.shield,
                    statusAmount = 2,
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