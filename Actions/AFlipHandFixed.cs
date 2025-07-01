using FMOD;
namespace TwosCompany.Actions {
    public class AFlipHandFixed : CardAction {
        public override void Begin(G g, State s, Combat c) {
            c.hand.Reverse();
            foreach (Card card in c.hand) {
                card.flipAnim = 1.0;
                card.flipped = !card.flipped;
            }
            Audio.Play(FSPRO.Event.CardHandling);
        }

        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> tooltips = new List<Tooltip>();
            tooltips.Add((Tooltip)new TTGlossary("action.flipHand", Array.Empty<object>()));
            return tooltips;
        }

        public override Icon? GetIcon(State s) => new Icon?(new Icon(StableSpr.icons_flipHand, new int?(), Colors.textMain));
    }

}
