using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Retribution : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.A ? 2 : 3,
                exhaust = false,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSpriteDown1" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = Status.payback,
                statusAmount = 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.B ? 3 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AExhaustSelf() {
                uuid = this.uuid,
                omitFromTooltips = false,
                disabled = Stance.Get(s) % 2 != 1,
            });

            actions.Add(new AStatus() {
                status = Status.tempPayback,
                statusAmount = upgrade == Upgrade.B ? 4 : 3,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 2,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override string Name() => "Retribution";
    }
}
