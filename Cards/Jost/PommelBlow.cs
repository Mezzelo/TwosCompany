using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class PommelBlow : Card, IJostCard, IJostFlippableCard {
        public bool markForFlop = false;
        bool IJostFlippableCard.markForFlop { get => markForFlop; set => markForFlop = value; }
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = true,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                moveEnemy = upgrade == Upgrade.B ? 2 : 0,
                stunEnemy = true,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                moveEnemy = upgrade == Upgrade.B ? 3 : 2,
                stunEnemy = upgrade == Upgrade.A,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override void OnExitCombat(State s, Combat c) {
            markForFlop = false;
            flipped = false;
        }

        public override string Name() => "Pommel Blow";
    }
}
