using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Cascade : Card {

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Cascade"].DescLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "left" : "right", GetDmg(state, 1));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Cascade"].DescALocKey ?? throw new Exception("Missing card description")),
                    flipped ? "left" : "right", GetDmg(state, 1));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Cascade"].DescBLocKey ?? throw new Exception("Missing card description")),
                    flipped ? "left" : "right", GetDmg(state, 2));

            return new CardData() {
                cost = 2,
                exhaust = upgrade == Upgrade.B,
                flippable = upgrade == Upgrade.A,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AMove() {
                dir = 0,
                ignoreHermes = true,
                targetPlayer = true,
                isRandom = false,
                timer = 0.0,
                omitFromTooltips = true,
            });
            actions.Add(new ACascadeAttack() {
                dir = flipped ? -1 : 1,
                timer = 0.0,
                damage = GetDmg(s, upgrade == Upgrade.B ? 2 : 1),
                sportsCounter = 0,
            });
            return actions;
        }

        public override string Name() => "Cascade";
    }
}
