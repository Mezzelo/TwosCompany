using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Backstep : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                retain = upgrade == Upgrade.A,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = 4,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = false,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());

            actions.Add(new AStatus() {
                status = Status.tempShield,
                statusAmount = 4,
                targetPlayer = false,
                disabled = Stance.Get(s) < 2,
            });
            actions.Add(new AStatus() {
                status = Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override string Name() => "Backstep";
    }
}
