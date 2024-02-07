using TwosCompany.Artifacts;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AEnableConduit : CardAction {
        public Conduit? conduit;
        public override void Begin(G g, State s, Combat c) {
            timer = 0.2;
            if (conduit == null)
                return;
            conduit.disabled = false;
            Audio.Play(FSPRO.Event.Status_PowerUp);
        }
    }
}