using CobaltCoreModding.Definitions.ExternalItems;
using System.Collections.Generic;

namespace TwosCompany.Actions {
    public class AStanceSwitch : CardAction {
        public override void Begin(G g, State state, Combat c) {

            ExternalStatus standFirm = Manifest.Statuses?["StandFirm"] ?? throw new Exception("status missing: standfirm");
            if (state.ship.Get((Status)standFirm.Id!) > 0)
                return;
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            bool hadOffense = true;
            bool switched = false;
            int initialTotal = state.ship.Get((Status)defensiveStance.Id!) + state.ship.Get((Status)offensiveStance.Id!);
            if (state.ship.Get((Status)defensiveStance.Id!) > 0) {
                if (state.ship.Get((Status)offensiveStance.Id!) == state.ship.Get((Status)defensiveStance.Id) && state.ship.Get(Status.boost) <= 0)
                    return;
                
                else
                    hadOffense = state.ship.Get((Status)offensiveStance.Id) > 0;
                state.ship.Set((Status)defensiveStance.Id!, Math.Max(state.ship.Get((Status)defensiveStance.Id!) - 1, 0));
                state.ship.Set((Status)offensiveStance.Id!, state.ship.Get((Status)offensiveStance.Id!) + 1 + state.ship.Get(Status.boost));
                if (state.ship.Get(Status.boost) > 0)
                    state.ship.Set(Status.boost, 0);
                Audio.Play(FSPRO.Event.Status_ShieldUp);
                switched = true;
            }
            if (state.ship.Get((Status)offensiveStance.Id!) > 0 && hadOffense) {
                state.ship.Set((Status)offensiveStance.Id!, Math.Max(state.ship.Get((Status)offensiveStance.Id!) - 1, 0));
                state.ship.Set((Status)defensiveStance.Id!, state.ship.Get((Status)defensiveStance.Id!) + 1 + state.ship.Get(Status.boost));
                if (state.ship.Get(Status.boost) > 0)
                    state.ship.Set(Status.boost, 0);
                if (!switched)
                    Audio.Play(FSPRO.Event.Status_ShieldUp);
                switched = true;
            }
            if (switched) {
                Manifest.EventHub.SignalEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch", new(state, c));
            }
        }
    }
}
