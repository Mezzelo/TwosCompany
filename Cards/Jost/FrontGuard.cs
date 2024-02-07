using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class FrontGuard : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 0 : 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade == Upgrade.A ? "Down1" : "") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) % 2 != 1
                });
            }

            actions.Add(new ADummyAction());

            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.overdrive : Status.shield,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });

            return actions;
        }

        public override string Name() => "Front Guard";
    }
}
