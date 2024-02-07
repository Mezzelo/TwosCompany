using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;
using TwosCompany.Cards.Ilya;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" },
        dontOffer = true, unreleased = true)]
    public class Fortress : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade == Upgrade.A ? "" : "Down1") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            ExternalStatus fortress = Manifest.Statuses?["Fortress"] ?? throw new Exception("status missing: fortress");
            actions.Add(new AStatus() {
                status = (Status)fortress.Id!,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
            });
            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
            });
            actions.Add(new ADummyAction());

            actions.Add(new AStatus() {
                status = Status.libra,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AAttack() {
                    damage = GetActualDamage(s, 1),
                    disabled = Stance.Get(s) < 2,
                });
            return actions;
        }

        public override string Name() => "Heartbeat";
    }
}
