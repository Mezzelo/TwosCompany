namespace TwosCompany.Actions {
    using System.Collections.Generic;

    public class AExhaustEntireHandImmediate : CardAction {
        public override void Begin(G g, State s, Combat c) {
            timer = 0.0;
            foreach (Card item in c.hand) {
                c.QueueImmediate(new AExhaustOtherCard {
                    uuid = item.uuid,
                    timer = -0.5
                });
            }
        }

        public override List<Tooltip> GetTooltips(State s) {
            return new List<Tooltip>
            {
            new TTGlossary("action.exhaustHand")
        };
        }

        public override Icon? GetIcon(State s) {
            return new Icon(Enum.Parse<Spr>("icons_exhaust"), null, Colors.textMain);
        }
    }
}
