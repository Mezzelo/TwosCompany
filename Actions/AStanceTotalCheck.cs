using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Extensions.Logging;
using System;
using TwosCompany.Artifacts;
using TwosCompany.Cards.Jost;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Actions {
    public class AStanceTotalCheck : CardAction {
        public MilitiaArmband? artif;
        internal static Manifest Instance => Manifest.Instance;
        public override void Begin(G g, State s, Combat c) {
            if (artif == null)
                return;
            int newTotal = s.ship.Get((Status)Manifest.Statuses?["DefensiveStance"].Id!) + s.ship.Get((Status)Manifest.Statuses?["OffensiveStance"].Id!);
            if (!artif.procced && newTotal > artif.prevTotal)
                artif.Proc(c);
            
            artif.prevTotal = newTotal;
        }
    }
}