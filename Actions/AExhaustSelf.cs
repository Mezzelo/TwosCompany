using FSPRO;

namespace TwosCompany.Actions {
    public class AExhaustSelf : CardAction {
        public int uuid;
        public override void Begin(G g, State s, Combat c) {

            // thanks to shockah for this snippet
            if (s.FindCard(uuid) is not { } card)
                return;

            s.RemoveCardFromWhereverItIs(uuid);
            c.SendCardToExhaust(s, card);
            Audio.Play(Event.CardHandling);
        }
        public override Icon? GetIcon(State s) => new Icon?(new Icon(Enum.Parse<Spr>("icons_exhaust"), new int?(), Colors.textMain));
        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary("cardtrait.exhaust") };
    }
}
