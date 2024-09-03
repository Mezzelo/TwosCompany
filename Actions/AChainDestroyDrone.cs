using Newtonsoft.Json.Linq;

namespace TwosCompany.Actions {
    public class AChainDestroyDrone : CardAction {

        public StuffBase? stuff;
        public StuffBase? stuff2;
        public bool playerDidIt = true;
        public bool hasSalvageArm = false;
        public bool noFx = false;
        public bool fxOverride = false;
        public override void Begin(G g, State s, Combat c) {
            if (stuff == null)
                return;
            if (noFx) {
                c.stuff.Remove(stuff.x);
                if (c.cardActions.Count > 0 && c.cardActions[c.cardActions.Count - 1] is AChainDestroyDrone)
                    timer = 0.0;
                else if (fxOverride) {
                    stuff.DoDestroyedEffect(s, c);
                    Audio.Play(FSPRO.Event.Hits_HitDrone);
                }
            }
            else {
                c.DestroyDroneAt(s, stuff.x, playerDidIt);
                Audio.Play(FSPRO.Event.Hits_HitDrone);
            }
            if (hasSalvageArm && c.cardActions.Count > 0 && c.cardActions[0] is AEnergy)
                c.cardActions[0].timer = 0.0;
            if (stuff2 != null) {
                c.DestroyDroneAt(s, stuff2.x, playerDidIt);
                if (hasSalvageArm && c.cardActions.Count > 0 && c.cardActions[0] is AEnergy)
                    c.cardActions[0].timer = 0.0;
            }
        }
    }
}
