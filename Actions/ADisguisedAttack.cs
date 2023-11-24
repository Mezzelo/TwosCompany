namespace TwosCompany.Actions {
    public class ADisguisedAttack : AAttack {
        public int? realDamage = 1;
        public bool? realStun = false;
        public bool? realPiercing = false;
        public bool disguised = true;

        public override void Begin(G g, State s, Combat c) {
            c.QueueImmediate(new AAttack() {
                damage = realDamage ?? this.damage,
                stunEnemy = realStun ?? this.stunEnemy,
                piercing = realPiercing ?? this.piercing,
            });
        }

        public override Icon? GetIcon(State s) {
            return base.GetIcon(s);
        }
        public override List<Tooltip> GetTooltips(State s) {
            if (disguised)
                return base.GetTooltips(s);
            else {
                return new AAttack() {
                    damage = realDamage ?? this.damage,
                    stunEnemy = realStun ?? this.stunEnemy,
                    piercing = realPiercing ?? this.piercing,
                }.GetTooltips(s);
            }
        }
    }
}
