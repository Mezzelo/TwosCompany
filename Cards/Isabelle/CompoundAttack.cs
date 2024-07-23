using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class CompoundAttack : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["CompoundAttack"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["CompoundAttack"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["CompoundAttack"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = cardText,
                exhaust = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            int count = 3;


            actions.Add(new ADrawOneOfTwo() {
                card1 = new Jab() {
                    upgrade = upgrade == Upgrade.B ? Upgrade.B : Upgrade.None, 
                    forTooltip = true
                },
                card2 = new Fleche() {
                    upgrade = upgrade == Upgrade.B ? Upgrade.B : Upgrade.None,
                    forTooltip = true
                },
                amount1 = count,
                amount2 = 1,
                disguise = true,
                perma = upgrade == Upgrade.B
            });
            return actions;
        }

        public override string Name() => "Compound Attack";
    }
}
