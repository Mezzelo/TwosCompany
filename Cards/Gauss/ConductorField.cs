using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ConductorField : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                retain = upgrade == Upgrade.A,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AConductorField() {
                dialogueSelector = c != null && c.stuff.Count > 0 ? ".mezz_conductorField" : "",
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AChainLightning() {
                    targetPlayer = false,
                    damage = GetDmg(s, 1),
                });
            return actions;
        }

        public override string Name() => "Conductor Field";
    }
}
