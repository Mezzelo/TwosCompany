namespace TwosCompany.Actions {
    using System.Collections.Generic;

    public class AExhaustEntireHandImmediate : AExhaustEntireHand {
        public override void Begin(G g, State s, Combat c) {
            timer = 0.0;
            foreach (Card item in c.hand) {
                c.QueueImmediate(new AExhaustOtherCard {
                    uuid = item.uuid,
                    timer = -0.5
                });
            }
        }
    }
}
