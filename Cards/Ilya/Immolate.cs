using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Immolate : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new StatCostAction() {
                    action = new AStatus() {
                        status = Status.shield,
                        targetPlayer = true,
                        statusAmount = 2,
                    },
                    statusReq = Status.heat,
                    statusCost = 2,
                    cumulative = 0,
                    first = true,
                });
            actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    damage = GetDmg(s, upgrade == Upgrade.A ? 1 : 5),
                    fast = upgrade == Upgrade.A,
                    dialogueSelector = upgrade == Upgrade.A ? null : ".mezz_immolate"
                },
                statusReq = Status.heat,
                statusCost = upgrade == Upgrade.A ? 1 : (upgrade == Upgrade.B ? 2 : 3),
                cumulative = upgrade == Upgrade.B ? 2 : 0,
                first = upgrade != Upgrade.B
            });
            if (upgrade == Upgrade.A)
                for (int i = 0; i < 2; i++)
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            damage = GetDmg(s, 2),
                            fast = true,
                            dialogueSelector = i == 1 ? ".mezz_immolate" : null
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = i + 1,
                    });
            return actions;
        }

        public override string Name() => "Immolate";
    }
}
