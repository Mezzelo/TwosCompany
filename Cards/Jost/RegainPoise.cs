using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class RegainPoise : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = upgrade == Upgrade.A,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            actions.Add(new AStatus() {
                status = (Status) defensiveStance.Id!,
                statusAmount = 1,
                mode = upgrade == Upgrade.B ? AStatusMode.Add : AStatusMode.Set,
                targetPlayer = true,
                dialogueSelector = Stance.Get(s) == 0 ? ".mezz_offBalance" : null,
            });
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: defensivestance");
            actions.Add(new AStatus() {
                status = (Status) offensiveStance.Id!,
                statusAmount = 0,
                mode = AStatusMode.Set,
                targetPlayer = true,
            });
            actions.Add(new ADrawCard {
                count = 2,
            });

            return actions;
        }

        public override string Name() => "Regain Poise";
    }
}
