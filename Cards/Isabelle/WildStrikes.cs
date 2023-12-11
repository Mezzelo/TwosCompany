namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class WildStrikes : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    fast = true,
                });
            actions.Add(new AMove() {
                dir = 2,
                targetPlayer = true,
                isRandom = true,
            });
            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
                fast = true,
            });
            if (upgrade != Upgrade.A) {
                actions.Add(new AMove() {
                    dir = 1,
                    targetPlayer = true,
                    isRandom = true
                });
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    fast = true,
                });
            } else {
                actions.Add(new AAddCard() {
                    card = new WildStrike(),
                    destination = CardDestination.Hand,
                    showCardTraitTooltips = false
                });
                actions.Add(new AAddCard() {
                    card = new WildDodge(),
                    destination = CardDestination.Hand,
                    showCardTraitTooltips = false
                }) ;
            }
            return actions;
        }

        public override string Name() => "Wild Strikes";
    }
}
