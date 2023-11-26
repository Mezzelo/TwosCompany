using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Couch : Card {

        public int initialX = 0;

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
                cost = upgrade == Upgrade.B ? 3 : 2,
            description = cardText,
                retain = upgrade == Upgrade.A
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
                return " <c=textMain>(</c><c=hurt>" + GetDistance(s).ToString() + "</c><c=maintext>)</c>";
            }
            else
                return "";
        }
        private int GetDistance(State s) {
            if (s.route is Combat)
                return Math.Abs(initialX - s.ship.x);
            return 0;
        }


        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAttack() {
                damage = GetDmg(s, GetDistance(s)),
            });
            return actions;
        }
        public override void OnExitCombat(State s, Combat c) {
            // roundDrawn = 0;
            initialX = 0;
        }
        public override void OnDraw(State s, Combat c) {
            // roundDrawn = c.turn;
            initialX = s.ship.x;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            // roundDrawn = 0;
        }
        public override void OnDiscard(State s, Combat c) {
            // roundDrawn = 0;
        }


        public override string Name() => "Couch";
    }
}
