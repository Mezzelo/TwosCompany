namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Remise : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Remise"].DescLocKey ?? throw new Exception("Missing card description")),
                    IncomingString(state, true), IncomingString(state, false));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Remise"].DescALocKey ?? throw new Exception("Missing card description")),
                    IncomingString(state, true), IncomingString(state, false));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Remise"].DescBLocKey ?? throw new Exception("Missing card description")),
                    IncomingString(state, true), IncomingString(state, false));

            return new CardData() {
                cost = 3,
                description = cardText,
                retain = upgrade == Upgrade.A
            };
        }
        private string IncomingString(State s, bool half) {
            int incomingTotal = 0;
            if (s.route is Combat route) {
                foreach (Part part in route.otherShip.parts) {
                    if (part.intent is IntentAttack intent)
                        incomingTotal += intent.multiHit;
                }
                if (half)
                    incomingTotal /= 2;
            }
            else {
                return half ? "<c=status>X/2</c>" : "<c=hurt>X</c>";
            }
            if (half)
                return "half (<c=status>" + incomingTotal.ToString() + "</c>)";
            else
                return incomingTotal.ToString();
        }
        private int GetIncomingTotal(State s) {
            int incomingTotal = 0;
            if (s.route is Combat route) {
                foreach (Part part in route.otherShip.parts) {
                    if (part.intent is IntentAttack intent)
                        incomingTotal += intent.multiHit;
                }
            }
            return incomingTotal;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            int incoming = GetIncomingTotal(s);


            actions.Add(new AStatus() {
                status = Status.evade,
                statusAmount = incoming / 2,
                targetPlayer = true,
                dialogueSelector = incoming > 2 ? ".mezz_remise" : null,
            });
            if (upgrade == Upgrade.B)
                for (int i = 0; i < incoming; i++) {
                    actions.Add(new AAttack() {
                        damage = GetDmg(s, 1),
                    });
                }
            else
                actions.Add(new AAttack() {
                    damage = GetDmg(s, incoming),
                });
            return actions;
        }

        public override string Name() => "Remise";
    }
}
