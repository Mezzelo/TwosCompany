using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true, extraGlossary = new string[] { "action.StanceCard" })]
    public class HackAndSlash : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 0,
                temporary = true,
                exhaust = upgrade != Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSpriteUp1" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
                piercing = upgrade == Upgrade.A,
                disabled = Stance.Get(s) % 2 != 1,
                dialogueSelector = Stance.Get(s) % 2 != 1 ? null : ".mezz_risingFlame",
            });

            actions.Add(new ADummyAction());

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
                piercing = upgrade == Upgrade.A,
                disabled = Stance.Get(s) < 2,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
                piercing = upgrade == Upgrade.A,
                disabled = Stance.Get(s) < 2,
                dialogueSelector = Stance.Get(s) < 2 ? null : ".mezz_risingFlame",
            });

            return actions;
        }

        public override string Name() => "Hack and Slash";
    }
}
