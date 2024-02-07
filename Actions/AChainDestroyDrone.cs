namespace TwosCompany.Actions {
    public class AChainDestroyDrone : CardAction {

        public StuffBase? stuff;
        public StuffBase? stuff2;
        public bool playerDidIt = true;
        public bool hasSalvageArm = false;

        public override void Begin(G g, State s, Combat c) {
            if (stuff == null)
                return;
            c.DestroyDroneAt(s, stuff.x, playerDidIt);
            if (hasSalvageArm && c.cardActions.Count > 0 && c.cardActions[0] is AEnergy)
                c.cardActions[0].timer = 0.0;
            if (stuff2 != null) {
                c.DestroyDroneAt(s, stuff2.x, playerDidIt);
                if (hasSalvageArm && c.cardActions.Count > 0 && c.cardActions[0] is AEnergy)
                    c.cardActions[0].timer = 0.0;
            }
            Audio.Play(FSPRO.Event.Hits_HitDrone);
        }
    }
}
