using System.Collections.Generic;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Couch : Card {

        public int dist = 0;
        public bool inHand = false;
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Couch"].DescLocKey ?? throw new Exception("Missing card description")),
                    GetDistanceString(state));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Couch"].DescALocKey ?? throw new Exception("Missing card description")),
                    GetDistanceString(state));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Couch"].DescBLocKey ?? throw new Exception("Missing card description")),
                    GetDistanceString(state));

            return new CardData() {
                cost = upgrade == Upgrade.None ? 1 : 2,
                exhaust = upgrade == Upgrade.B,
                retain = upgrade == Upgrade.B,
                description = cardText,
            };
        }
        private int GetRound(State s) {
            int currentRound = 0;
            if (s.route is Combat)
                currentRound = ((Combat)s.route).turn;
            return currentRound;
        }

        private string GetDistanceString(State s) {
            if (s.route is Combat) {
                return " <c=textMain>(</c><c=hurt>" + GetDmg(s, GetDistance(s)).ToString() + "</c><c=maintext>)</c>";
            }
            else
                return "";
        }
        private int GetDistance(State s) {
            if (s.route is Combat)
                return dist * (upgrade == Upgrade.A ? 2 : 1);
            return 0;
        }


        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAttack() {
                damage = GetDmg(s, GetDistance(s)),
                // dialogueSelector = GetDmg(s, GetDistance(s)) > 1 ? ".mezz_couch" : null,
            });
            return actions;
        }
        public override void OnExitCombat(State s, Combat c) => dist = 0;
        public override void OnDraw(State s, Combat c) => dist = 0;

        public override string Name() => "Couch";
    }
}
