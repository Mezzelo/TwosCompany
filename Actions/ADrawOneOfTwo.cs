﻿using FSPRO;
using TwosCompany.Cards;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Actions {
    public class ADrawOneOfTwo : CardAction {
        public Card? card1;
        public Card? card2;
        public int amount1 = 1;
        public int amount2 = 1;
        public bool disguise = true;
        public bool perma = false;

        public double waitBeforeMoving = 0.4;
        public override void Begin(G g, State s, Combat c) {
            if (s.route is not Combat)
                return;
            timer = waitBeforeMoving + 0.2;
            if (card1 == null)
                return;
            if (card2 == null)
                return;

            double chance = amount1 / ((double) amount1 + amount2);
            bool is2 = s.rngActions.Next() > chance;
            Manifest.CombatQolApi?.MarkSafeRngAdvance(c, ExternalAPI.ICombatQolApi.RngTypes.ACTION);
            Card newCard = (is2 ? card2 : card1).CopyWithNewId();
            if (is2)
                amount2--;
            else
                amount1--;
            if (disguise && newCard is IDisguisedCard) {
                ((IDisguisedCard)newCard).disguised = true;
                ((IDisguisedCard)newCard).forTooltip = false;
            }
            newCard.pos = new Vec(G.screenSize.x * 0.5 - 30.0, 30.0);
            newCard.waitBeforeMoving = waitBeforeMoving;
            newCard.drawAnim = 1.0;
            foreach (Artifact item in g.state.EnumerateAllArtifacts()) {
                item.OnPlayerRecieveCardMidCombat(g.state, c, newCard);
            }
            s.OnHasCard(newCard);
            c?.SendCardToHand(s, newCard);
            Audio.Play(Event.CardHandling);
            s.DebugSafeIdCheck();
            if (amount1 > 0 || amount2 > 0) {
                c?.QueueImmediate(new ADrawOneOfTwo() {
                    card1 = this.card1,
                    card2 = this.card2,
                    amount1 = this.amount1,
                    amount2 = this.amount2,
                    disguise = this.disguise
                });
            }
        }
        public override Icon? GetIcon(State s) => new Icon?(new Icon(StableSpr.icons_addCard, 1, Colors.textMain));
        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary("action.addCard", "<c=deck>hand</c>"));
            list.Add(new TTCard {
                card = card1?? new Jab(),
                showCardTraitTooltips = false,
            });
            list.Add(new TTGlossary(Manifest.Glossary[perma ? "DisguisedPermaHint" : "DisguisedHint"]?.Head ??
            throw new Exception("missing glossary entry: disguisedHint")));
            list.Add(new TTCard {
                card = card2?? new Jab(),
                showCardTraitTooltips = false,
            });
            return list;
        }
    }
}