using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.B }, dontOffer = true, dontLoc = true)]
    public class Fleche : Card, DisguisedCard {
        public bool disguised = false;
        public bool wasPlayed = false;
        bool DisguisedCard.disguised { get => disguised;  set => disguised = value; }

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                temporary = true,
                exhaust = upgrade != Upgrade.B,
                recycle = upgrade == Upgrade.B,
                art = new Spr?((Spr)((disguised ? Manifest.Sprites["JabCardSprite"] : Manifest.Sprites["FlecheCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADisguisedAttack() {
                damage = GetDmg(s, this.disguised ? 1 : (upgrade == Upgrade.B ? 3 : 5)),
                stunEnemy = this.disguised ? false : true,
                realDamage = GetDmg(s, 5),
                realStun = true,
                disguised = this.disguised
            });
            actions.Add(new ADisguisedStatus() {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = this.disguised ? 1 : -3,
                realAmount = -3,
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
        public override string Name() => disguised ? "Jab?" : "Fleche";
    }
}
