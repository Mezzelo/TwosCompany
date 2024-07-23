using CobaltCoreModding.Definitions.ExternalItems;
using System.Collections.Generic;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class FreedomOfMovement : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
                retain = true,
                exhaust = upgrade != Upgrade.B,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses?["Superposition"].Id!,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true,
            });

            return actions;
        }

        public override string Name() => "Freedom of Movement";
    }
}
