using static System.Net.Mime.MediaTypeNames;

namespace TwosCompany.Actions {
    public class AForceAttackApply : CardAction {
        public int x;
        public IntentAttack? intent;
        public override void Begin(G g, State s, Combat c) {
            if (intent == null)
                intent = (IntentAttack) c.otherShip.parts[x].intent!;
            AAttack aAttack = new AAttack {
                damage = Card.GetActualDamage(s, intent.damage, targetPlayer: true),
                status = intent.status,
                statusAmount = (intent.status.HasValue ? intent.statusAmount : 0),
                cardOnHit = intent.cardOnHit,
                destination = intent.destination,
                targetPlayer = true,
                fromX = x
            };
            if (intent.fast || intent.multiHit > 1)
                aAttack.fast = true;
            for (int i = 0; i < intent.multiHit; i++)
                c.QueueImmediate(aAttack);
            
            c.otherShip.parts[x].intent = null;
            timer = 0.0;
        }
    }
}