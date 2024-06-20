using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class StrikeTwice : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            if (upgrade != Upgrade.B)
                actions.Add(new ABubbleField());
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, 1),
            });
            if (upgrade == Upgrade.A)
                actions.Add(new ABubbleField());
            actions.Add(new AChainLightning() {
                targetPlayer = false,
                damage = GetDmg(s, 1),
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ABubbleField());
            return actions;
        }

        public override string Name() => "Strike Twice";
    }
}
