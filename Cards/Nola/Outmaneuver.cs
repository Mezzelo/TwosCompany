namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Outmaneuver : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Outmaneuver"].DescLocKey ?? throw new Exception("Missing card description")),
                    "1", IncomingString(state));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Outmaneuver"].DescALocKey ?? throw new Exception("Missing card description")),
                    "1", IncomingString(state));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Outmaneuver"].DescBLocKey ?? throw new Exception("Missing card description")),
                    "2", IncomingString(state));

            return new CardData() {
                cost = 3,
                description = cardText,
                retain = upgrade == Upgrade.A,
                exhaust = upgrade == Upgrade.B
            };
        }
        private string IncomingString(State s) {
            if (s.route is Combat) {
                return " <c=textMain>(</c><c=status>" + GetIncomingTotal(s).ToString() + "</c><c=maintext>)</c>";
            }
            else
                return "";
        }
        private int GetIncomingTotal(State s) {
            int incomingTotal = 0;
            if (s.route is Combat route) {
                for (int i = 0; i < s.ship.parts.Count; i++) {
                    if (i < route.otherShip.x)
                        continue;
                    else if (i >= route.otherShip.x + route.otherShip.parts.Count)
                        break;
                    if (route.otherShip.parts[i].intent is IntentAttack)
                        incomingTotal++;
                }
            }
            return incomingTotal * (upgrade == Upgrade.B ? 2 : 1);
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            int incoming = GetIncomingTotal(s);
            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = incoming * GetIncomingTotal(s),
                targetPlayer = true,
            });
            return actions;
        }

        public override string Name() => "Outmaneuver";
    }
}
