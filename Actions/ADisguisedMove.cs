namespace TwosCompany.Actions {
    public class ADisguisedMove : AMove {
        public bool? realTargetPlayer;
        public int? realDir;
        public bool? realIsRandom;
        public bool disguised = true;

        public override void Begin(G g, State s, Combat c) {
            if (realDir == 0)
                return;
            c.QueueImmediate(new AMove() {
                dir = realDir ?? dir,
                targetPlayer = realTargetPlayer ?? this.targetPlayer,
                isRandom = realIsRandom ?? this.isRandom,
            });
        }

        public override List<Tooltip> GetTooltips(State s) {
            if (disguised)
                return base.GetTooltips(s);
            else {
                return new AMove() {
                    dir = realDir ?? dir,
                    targetPlayer = realTargetPlayer ?? this.targetPlayer,
                    isRandom = realIsRandom ?? this.isRandom,
                }.GetTooltips(s);
            }
        }
    }
}
