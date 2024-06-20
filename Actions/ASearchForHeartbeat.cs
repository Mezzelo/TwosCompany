using FMOD;
using TwosCompany.Cards.Jost;
namespace TwosCompany.Actions {
    public class ASearchForHeartbeat : CardAction {

        public override void Begin(G g, State s, Combat c) {
            if (c == null)
                return;
            this.timer = 0.0;
            int count = 0;
            bool full = false;
            List<Card> cardList = s.deck;
            for (int drawIdx = cardList.Count - 1; drawIdx >= 0; --drawIdx) {
                Card card = cardList[drawIdx];
                if (card is not Heartbeat)
                    continue;
                if (c.hand.Count >= 10) {
                    c.PulseFullHandWarning();
                    full = true;
                    break;
                }
                ++count;
                Heartbeat h = (Heartbeat) card;
                if (!(h.retainOverride == true)) {
                    h.breatheInRetain = true;
                    h.retainOverride = true;
                }
                c.DrawCardIdx(s, drawIdx, CardDestination.Deck);
            }
            cardList = c.discard;
            for (int drawIdx = cardList.Count - 1; drawIdx >= 0 && !full; --drawIdx) {
                Card card = cardList[drawIdx];
                if (card is not Heartbeat)
                    continue;
                if (c.hand.Count >= 10) {
                    c.PulseFullHandWarning();
                    full = true;
                    break;
                }
                ++count;
                Heartbeat h = (Heartbeat)card;
                if (!(h.retainOverride == true)) {
                    h.breatheInRetain = true;
                    h.retainOverride = true;
                }
                c.DrawCardIdx(s, drawIdx, CardDestination.Discard);
            }
            if (count > 0) {
                this.timer = 0.4;
                foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts())
                    enumerateAllArtifact.OnDrawCard(s, c, count);
                Audio.Play(FSPRO.Event.CardHandling);
            }
            cardList = c.exhausted;
            for (int drawIdx = cardList.Count - 1; drawIdx >= 0 && !full; --drawIdx) {
                Card card = cardList[drawIdx];
                if (card is not Heartbeat)
                    continue;
                if (c.hand.Count >= 10) {
                    c.PulseFullHandWarning();
                    full = true;
                    break;
                }
                ++count;
                Heartbeat h = (Heartbeat)card;
                if (!(h.retainOverride == true)) {
                    h.breatheInRetain = true;
                    h.retainOverride = true;
                }
                c.DrawCardIdx(s, drawIdx, CardDestination.Exhaust);
            }
            if (count > 0) {
                this.timer = 0.4;
                foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts())
                    enumerateAllArtifact.OnDrawCard(s, c, count);
                Audio.Play(FSPRO.Event.CardHandling);
            }
        }
    }

}
