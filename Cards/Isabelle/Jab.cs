using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class Jab : Card, IDisguisedCard {
        public bool disguised = false;
        public bool forTooltip = false;
        // public bool wasPlayed = false;
        bool IDisguisedCard.disguised { get => disguised; set => disguised = value; }
        bool IDisguisedCard.forTooltip { get => forTooltip; set => forTooltip = value; }
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
            if (disguised)
                actions.Add(new ADisguisedHint() {
                    perma = upgrade == Upgrade.B,
                    actualCard = new List<TTCard> { new TTCard() { card = new Fleche() { upgrade = this.upgrade },
                        showCardTraitTooltips = false, } },
                });
            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 2 : 1),
                omitFromTooltips = forTooltip,
                dialogueSelector = ".mezz_jab"
            });
            actions.Add(new AStatus() {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = 1,
                omitFromTooltips = forTooltip
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
            // wasPlayed = true;
            disguised = upgrade == Upgrade.B;
        }

        public override string Name() => disguised ? "Jab?" : "Jab";
    }
}
