using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class ReactiveDefense : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade == Upgrade.None ? "Up1" : "") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.maxShield,
                statusAmount = 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });

            if (upgrade != Upgrade.None)
                actions.Add(new AStatus() {
                    status = Status.shield,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) % 2 != 1
                });
            
            actions.Add(new ADummyAction());

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.B ? 4 : 1),
                disabled = Stance.Get(s) < 2,
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.maxShield : Status.shield,
                statusAmount = upgrade == Upgrade.B ? -1 : (upgrade == Upgrade.A ? 2 : 1),
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override string Name() => "Reactive Defense";
    }
}
