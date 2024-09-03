namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class HeatRecycler : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status)Manifest.Statuses?["HeatFeedback"].Id!,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true,
            });
            if (upgrade == Upgrade.A) {
                actions.Add(new ADrawCard() {
                    count = 2,
                });
            }
            else if (upgrade == Upgrade.B) {
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 0),
                    fast = true,
                });
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    fast = true,
                });
            }
            return actions;
        }

        public override string Name() => "Heat Recycler";
    }
}
