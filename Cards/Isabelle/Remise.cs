namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Remise : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Remise"].DescLocKey ?? throw new Exception("Missing card description")),
                    GetIncomingTotal(state).ToString());
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Remise"].DescALocKey ?? throw new Exception("Missing card description")),
                    GetIncomingTotal(state).ToString());
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["Remise"].DescBLocKey ?? throw new Exception("Missing card description")),
                    GetIncomingTotal(state).ToString());

            return new CardData() {
                cost = 2,
                description = cardText,
                retain = upgrade == Upgrade.A
            };
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
                statusAmount = incoming,
                targetPlayer = true,
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
