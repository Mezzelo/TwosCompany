namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.uncommon, dontOffer = true, unreleased = true, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
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
                cost = 2,
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
            if (s.route is Combat c) {
                for (int i = 0; i < c.otherShip.parts.Count; i++) {
                    if (i + c.otherShip.x < s.ship.x)
                        continue;
                    else if (i + c.otherShip.x >= s.ship.x + s.ship.parts.Count)
                        break;
                    if (c.otherShip.parts[i].intent is IntentAttack)
                        incomingTotal++;
                }
            }
            return incomingTotal * (upgrade == Upgrade.B ? 2 : 1);
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = GetIncomingTotal(s),
                targetPlayer = true,
                dialogueSelector = GetIncomingTotal(s) > 2 ? ".mezz_outmaneuver" : null
            });
            return actions;
        }

        public override string Name() => "Out Maneuver";
    }
}
