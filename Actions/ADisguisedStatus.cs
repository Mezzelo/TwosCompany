namespace TwosCompany.Actions {
    public class ADisguisedStatus : AStatus {
        public Status? realStatus;
        public bool? realTargetPlayer;
        public AStatusMode? realMode;
        public int? realAmount;
        public bool disguised = true;

        public override void Begin(G g, State s, Combat c) {
            if (realAmount == 0)
                return;
            c.QueueImmediate(new AStatus() {
                status = realStatus ?? status,
                targetPlayer = realTargetPlayer ?? this.targetPlayer,
                mode = realMode ?? this.mode,
                statusAmount = realAmount ?? this.statusAmount
            });
        }

        public override Icon? GetIcon(State s) {
            if (disguised)
                return base.GetIcon(s);
            else {
                int? number = ((realMode ?? this.mode) == AStatusMode.Set) ? null : new int?(realAmount ?? this.statusAmount);
                Spr icon = DB.statuses[realStatus ?? this.status].icon;
                return new Icon(icon, number, Colors.textMain);
            }
        }
        public override List<Tooltip> GetTooltips(State s) {
            if (disguised)
                return base.GetTooltips(s);
            else {
                return new AStatus() {
                    status = realStatus ?? status,
                    targetPlayer = realTargetPlayer ?? this.targetPlayer,
                    mode = realMode ?? this.mode,
                    statusAmount = realAmount ?? this.statusAmount
                }.GetTooltips(s);
            }
        }
    }
}
