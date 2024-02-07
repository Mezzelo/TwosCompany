using FMOD;
namespace TwosCompany.Actions {
    public class ASearchForSpawn : CardAction {

        public int count = 1;

        public override void Begin(G g, State s, Combat c) {
            if (c == null)
                return;
            this.timer = 0.0;
            bool full = false;
            List<Card> cardList = s.deck;
            List<int> spawnList = new List<int>();
            int deck = 0;
            int drawn = 0;
            for (int drawIdx = cardList.Count - 1; drawIdx >= 0; --drawIdx) {
                Card card = cardList[drawIdx];
                bool isSpawn = false;
                List<CardAction> actions = card.GetActions(s, c);
                foreach (CardAction action in actions) {
                    if (action is ASpawn) {
                        isSpawn = true;
                        break;
                    }
                }
                if (!isSpawn)
                    continue;
                ++deck;
                spawnList.Add(drawIdx);
            }
            cardList = c.discard;
            for (int drawIdx = cardList.Count - 1; drawIdx >= 0 && !full; --drawIdx) {
                Card card = cardList[drawIdx];
                bool isSpawn = false;
                List<CardAction> actions = card.GetActions(s, c);
                foreach (CardAction action in actions) {
                    if (action is ASpawn) {
                        isSpawn = true;
                        break;
                    }
                }
                if (!isSpawn)
                    continue;
                spawnList.Add(drawIdx);
            }
            if (spawnList.Count > 0) {
                for (int i = 0; i < count && spawnList.Count > 0; i++) {
                    int n = s.rngActions.NextInt() % spawnList.Count;
                    if (c.hand.Count >= 10) {
                        c.PulseFullHandWarning();
                        break;
                    } else {
                        drawn++;
                        if (i < count - 1)
                            for (int t = (n < deck ? 0 : deck); t < (n < deck ? deck : spawnList.Count); t++) {
                                if (spawnList[t] > spawnList[n] && t != n)
                                    spawnList[t]--;
                            }
                        c.DrawCardIdx(s, spawnList[n], n >= deck ? CardDestination.Discard : CardDestination.Deck);
                        if (i == count - 1)
                            break;
                        spawnList.RemoveAt(n);
                        if (n < deck)
                            deck--;
                    }
                }
            }
            if (drawn > 0) {
                foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts())
                    enumerateAllArtifact.OnDrawCard(s, c, drawn);
                Audio.Play(FSPRO.Event.CardHandling);
            }
        }

        public override Icon? GetIcon(State s) => new Icon?(new Icon(Enum.Parse<Spr>("icons_searchCard"), count, Colors.textMain));
    }

}
