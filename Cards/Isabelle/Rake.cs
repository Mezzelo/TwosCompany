using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Rake : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade != Upgrade.B ? 4 : 1,
                exhaust = upgrade != Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? (Status) Manifest.Statuses["TempStrafe"].Id! : Status.strafe,
                statusAmount = 1,
                targetPlayer = true,
                // dialogueSelector = ".mezz_rake",
            });
            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = 2,
                targetPlayer = true
            });
            if (upgrade != Upgrade.A)
                actions.Add(new AStatus() {
                    status = Status.loseEvadeNextTurn,
                    statusAmount = 1,
                    targetPlayer = true
                });
            return actions;
        }

        public override string Name() => "Rake";
    }
}
