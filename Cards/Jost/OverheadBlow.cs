using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[]{ "action.StanceCard"} )]
    public class OverheadBlow : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 2),
                fast = upgrade == Upgrade.B,
                disabled = Stance.Get(s) % 2 != 1
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    fast = true,
                    disabled = Stance.Get(s) % 2 != 1
                });

            actions.Add(new ADummyAction());

            actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    fast = upgrade == Upgrade.B,
                    damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 5),
                },
                statusReq = Status.shield,
                statusCost = upgrade == Upgrade.None ? 2 : 1,
                disabled = Stance.Get(s) < 2,
                first = true,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new StatCostAttack() {
                    action = new AAttack() {
                        fast = true,
                        damage = GetDmg(s, 5),
                    },
                    statusReq = Status.shield,
                    statusCost = upgrade == Upgrade.A ? 1 : 2,
                    disabled = Stance.Get(s) < 2,
                    cumulative = 1
                });
            return actions;
        }

        public override string Name() => "Overhead Blow";
    }
}
