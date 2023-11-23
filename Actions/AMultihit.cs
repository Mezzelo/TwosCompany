namespace TwosCompany.Actions {
    public class AMultiHit : CardAction {
        public bool checkHit() {
            return true;
        }
        public override void Begin(G g, State s, Combat c) {

            if (!(s.route is Combat))
                return;
            var cannonIndices = s.ship.parts.Select((part, index) => new { index, part }).Where(e => e.part.type == PType.cannon && e.part.active);

                // ;
                // if (cannons.Count() == 0)
                //     return;


            // if ()
            // c.QueueImmediate
        }
    }
}