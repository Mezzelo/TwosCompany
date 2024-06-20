using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HookAndDrag : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                flippable = upgrade == Upgrade.A,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["HookAndDragCardSpriteFlip"] : Manifest.Sprites["HookAndDragCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AMoveEnemy() {
                dir = -4,
                targetPlayer = false,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 3),
            });
            if (upgrade == Upgrade.B) {
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 2),
                    moveEnemy = 2
                });
            }
            return actions;
        }

        public override string Name() => "Hook And Drag";
    }
}
