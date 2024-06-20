using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class SweepingStrikes : Card, IJostCard, IJostFlippableCard {
        public bool markForFlop = false;
        bool IJostFlippableCard.markForFlop { get => markForFlop; set => markForFlop = value; }
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                moveEnemy = upgrade == Upgrade.B ? 3 : 2,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());
            if (upgrade == Upgrade.B) {

            }
            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                moveEnemy = 1,
                disabled = Stance.Get(s) < 2,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2),
                moveEnemy = upgrade == Upgrade.B ? -3 : -2,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override string Name() => "Sweeping Strikes";
    }
}
