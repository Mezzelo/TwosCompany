namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.B }, dontOffer = true)]
    public class Jab : Card, DisguisedCard {
        public bool disguised = false;
        public bool wasPlayed = false;
        bool DisguisedCard.disguised { get => disguised; set => disguised = value; }
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                temporary = true,
                exhaust = upgrade != Upgrade.B,
                recycle = upgrade == Upgrade.B
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAttack() {
                damage = GetDmg(s, 1),
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = 1,
            });
            return actions;
        }

        /*
        public override void OnDraw(State s, Combat c) {
        }
        public override void OnDiscard(State s, Combat c) {
        }
        */
        public override void AfterWasPlayed(State state, Combat c) {
            wasPlayed = true;
            disguised = false;
        }

        public override string Name() => disguised ? "Jab?" : "Jab";
    }
}
