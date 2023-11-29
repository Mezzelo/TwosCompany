namespace TwosCompany.Actions {
    public class APlayAllCards : CardAction {
        public bool leftToRight;
        public int gainHeat;
        public int index;
        public int[]? originalHand;
        public bool firstPlay;
        public override void Begin(G g, State s, Combat c) {
            if (firstPlay) {
                index = leftToRight ? 0 : c.hand.Count - 1;
                originalHand = new int[c.hand.Count];
                for (int i = 0; i < c.hand.Count; i++)
                    originalHand[i] = c.hand[i].uuid;
            }
            foreach (Card compare in c.hand) {
                if (compare.uuid == (originalHand?[index] ?? throw new Exception("no hand in playallcards"))) {
                    c.TryPlayCard(s, compare, true);
                    Audio.Play(FSPRO.Event.CardHandling);
                    if (gainHeat != 0)
                        c.QueueImmediate(new AStatus() {
                            status = Status.heat,
                            statusAmount = 1,
                            targetPlayer = true
                        });
                    /*
                    c.Queue(new ADelay() {
                        time = 0.1
                    });*/
                    break;
                }
            }
            if (leftToRight && index < (originalHand ?? throw new Exception("no hand in playallcards")).Length - 1 || !leftToRight && index > 0)
                c.Queue(new APlayAllCards() {
                    leftToRight = this.leftToRight,
                    gainHeat = this.gainHeat,
                    index = this.index + (leftToRight ? 1 : -1),
                    originalHand = this.originalHand,
                    firstPlay = false
                });
        }
        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = new List<Tooltip>() { new TTGlossary("action.bypass") };
            if (gainHeat > 0) {
                list.Add(new TTGlossary("status.heat", 1));
            }
            return list;
        }
    public override Icon? GetIcon(State s) => new Icon?(new Icon(Enum.Parse<Spr>("icons_bypass"), 0, Colors.textMain));
    }
}