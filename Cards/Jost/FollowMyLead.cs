using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" },
        unreleased = true, dontOffer = true)]
    public class FollowMyLead : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = upgrade != Upgrade.B,
                exhaust = upgrade != Upgrade.B,
                buoyant = true,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade == Upgrade.None ? "Down1" : "") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADrawCard() {
                count = 1,
                disabled = Stance.Get(s) % 2 != 1,
            });
            ExternalStatus onslaughtStatus = Manifest.Statuses?["Onslaught"] ?? throw new Exception("status missing: onslaught");
            if (upgrade != Upgrade.B)
                actions.Add(new AStatus() {
                    status = onslaughtStatus.Id != null ? (Status)onslaughtStatus.Id : Status.drawNextTurn,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) % 2 != 1,
                });

            actions.Add(new ADummyAction());

            actions.Add(new AStatus() {
                status = onslaughtStatus.Id != null ? (Status) onslaughtStatus.Id : Status.drawNextTurn,
                statusAmount = 5,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });
            if (upgrade == Upgrade.A)
            actions.Add(new AEnergy() {
                changeAmount = 1,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override string Name() => "Follow My Lead";
    }
}
