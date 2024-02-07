using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Challenge : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSpriteDown1" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.B ? 5 : 3,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
                dialogueSelector = Stance.Get(s) % 2 != 1 ? null : ".mezz_challenge",
            });
            actions.Add(new AStatus() {
                status = Status.maxShield,
                statusAmount = upgrade == Upgrade.B ? 2 : 1,
                targetPlayer = false,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.A ? 1 : (upgrade == Upgrade.B ? 3 : 2),
                targetPlayer = false,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
                dialogueSelector = Stance.Get(s) < 2 ? null : ".mezz_challenge",
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.powerdrive : Status.overdrive,
                statusAmount = upgrade == Upgrade.None ? 2 : 1,
                targetPlayer = false,
                disabled = Stance.Get(s) < 2,
            });

            return actions;
        }

        public override string Name() => "Challenge";
    }
}
