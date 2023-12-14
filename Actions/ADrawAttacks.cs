using System;

// TODO: i'll finish this card later lol
namespace TwosCompany.Actions {
    public class ADrawAttacks : CardAction {
        public bool drawNotDiscard = false;
        public override void Begin(G g, State s, Combat c) {
            if (c.hand.Count == 0)
                return;
            int num = 0;
            foreach (Card current in c.hand) {
                List<CardAction> actions = current.GetActions(s, c);
                bool isAttack = false;
                foreach (CardAction thisAction in actions) {
                    if (thisAction is AAttack) {
                        isAttack = true;
                        break;
                    }
                }
                if (!isAttack) {
                    s.RemoveCardFromWhereverItIs(current.uuid);
                    current.waitBeforeMoving = num++ * 0.05;
                    current.flipped = false;
                    current.OnDiscard(s, c);
                    c.SendCardToDiscard(s, current);
                }
            }
            Audio.Play(FSPRO.Event.CardHandling);
            /*
            if (num > 0) {
                //  && count < s.ship.statusEffects[(Status)onslaughtStatus.Id]
                for (int i = 0; i < num && (s.deck.Count > 0 && s.; i++) {
                    if (s.deck.Count == 0 )
                    Card selectCard = cardList[drawIdx];
                    if (selectCard.GetMeta().deck == card.GetMeta().deck) {
                        if (card.uuid != selectCard.uuid) {
                            if (__instance.hand.Count >= 10) {
                                __instance.PulseFullHandWarning();
                                break;
                            }
                            __instance.DrawCardIdx(s, drawIdx, CardDestination.Deck);
                            Audio.Play(FSPRO.Event.CardHandling);
                            count++;
                            s.ship.Set((Status)onslaughtStatus.Id, s.ship.Get((Status)onslaughtStatus.Id) - 1);
                            // continue;
                            break;
                        }
                    }
                }
            } */
        }
    }
}