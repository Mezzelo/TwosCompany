﻿using TwosCompany.Actions;

namespace TwosCompany.Cards.Isabelle {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, dontOffer = true)]
    public class Fleche : Card, IDisguisedCard {
        public bool disguised = false;
        public bool forTooltip = false;
        // public bool wasPlayed = false;
        bool IDisguisedCard.disguised { get => disguised;  set => disguised = value; }
        bool IDisguisedCard.forTooltip { get => forTooltip; set => forTooltip = value; }

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

            if (disguised)
                actions.Add(new ADisguisedHint() {
                    perma = upgrade == Upgrade.B,
                    actualCard = new List<TTCard> { new TTCard() { card = new Fleche() { upgrade = this.upgrade },
                        showCardTraitTooltips = false, } },
                });
            actions.Add(new ADisguisedAttack() {
                damage = GetDmg(s, this.disguised ? (upgrade == Upgrade.A ? 2 : 1) : (upgrade == Upgrade.A ? 5 : 4)),
                stunEnemy = this.disguised ? false : true,
                realDamage = GetDmg(s, (upgrade == Upgrade.A ? 5 : 4)),
                realStun = true,
                disguised = this.disguised,
                dialogueSelector = ".mezz_fleche"
            });
            // if (!(upgrade == Upgrade.B && !disguised))
                actions.Add(new ADisguisedStatus() {
                    targetPlayer = true,
                    status = Status.evade,
                    statusAmount = this.disguised ? 1 : -1,
                    realAmount = -1,
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
        public override string Name() => disguised ? "Jab?" : "Fleche";
    }
}
