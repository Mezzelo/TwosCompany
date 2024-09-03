using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class HighGuard : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade == Upgrade.B ? "" : "Down1") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?["FalseOpening"].Id!,
                statusAmount = 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new AStatus() {
                status = upgrade == Upgrade.B ? Status.tempShield : Status.shield,
                statusAmount = upgrade != Upgrade.None ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());

            actions.Add(new StatCostAction() {
                action = new AStatus() {
                    status = Status.overdrive,
                    statusAmount = 2,
                    targetPlayer = true,
                },
                statusReq = Status.shield,
                statusCost = 2,
                first = true,
                disabled = Stance.Get(s) < 2,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = Status.tempShield,
                    statusAmount = 2,
                    targetPlayer = true,
                    disabled = Stance.Get(s) < 2,
                });

            return actions;
        }

        public override string Name() => "High Guard";
    }
}
