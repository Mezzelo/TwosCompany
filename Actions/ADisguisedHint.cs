using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Actions {
    public class ADisguisedHint : CardAction {
        public bool perma = false;
        public List<Card>? actualCard;

        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites[perma ? "IconDisguisedPermaHint" : "IconDisguisedHint"].Id ??
            throw new Exception("missing icon")), null, Colors.status);

        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = new List<Tooltip>() {
            new TTGlossary(Manifest.Glossary[perma ? "DisguisedPermaHint" : "DisguisedHint"]?.Head ??
            throw new Exception("missing glossary entry: disguisedHint")) };
            if (actualCard != null) {
                foreach (Card selectedCard in actualCard) {
                    list.Add(new TTCard {
                        card = selectedCard,
                        showCardTraitTooltips = false,
                    });
                }
            }
            return list;
        }
    }
}