namespace TwosCompany.Actions {
    public class AVolleyChainLightning : CardAction {
        public AChainLightning? attack;

        public override void Begin(G g, State s, Combat c) {
            if (attack == null)
                return;
            timer = 0.0;
            attack.multiCannonVolley = true;
            List<AChainLightning> list = new List<AChainLightning>();
            int num = 0;
            foreach (Part part in s.ship.parts) {
                if (part.type == PType.missiles && part.active) {
                    attack.fromX = num;
                    list.Add(Mutil.DeepCopy(attack));
                }
                num++;
            }
            c.QueueImmediate(list);
        }
    }
}
