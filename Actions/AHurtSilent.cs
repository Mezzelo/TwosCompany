using FSPRO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.Actions {
    public class AHurtSilent : CardAction {

        public int hurtAmount = 0;
        public bool targetPlayer = true;
        public bool hurtShieldsFirst = false;
        public bool cannotKillYou = false;
        public override void Begin(G g, State s, Combat c) {
            timer = 0.0;
            if (hurtAmount == 0)
                return;
            c.QueueImmediate(new AHurt() {
                hurtAmount = this.hurtAmount,
                targetPlayer = this.targetPlayer,
                hurtShieldsFirst = this.hurtShieldsFirst,
                cannotKillYou = this.cannotKillYou,
            });
        }
        public override Icon? GetIcon(State s) {
            return new Icon(StableSpr.icons_hurt, hurtAmount, Colors.hurt);
        }
        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary("action.hurt", hurtAmount));
            return list;
        }
    }
}
