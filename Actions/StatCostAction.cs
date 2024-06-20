using System.Collections.Generic;

namespace TwosCompany.Actions {
    public class StatCostAction : CardAction, IStatCost {
        public Status statusReq;
        public int statusCost = 1;
        public int cumulative = 0;
        public CardAction? action;
        public bool first = false;
        Status IStatCost.statusReq { get => statusReq; }
        int IStatCost.statusCost { get => statusCost; }
        int IStatCost.cumulative { get => cumulative; }
        CardAction? IStatCost.action { get => action; }
        bool IStatCost.first { get => first; }
        public override void Begin(G g, State s, Combat c) {
            if (action == null)
                return;
            if (s.ship.Get(statusReq) >= statusCost) {
                s.ship.Add(statusReq, -this.statusCost);
                if (statusReq == Status.evade)
                    Audio.Play(FSPRO.Event.Status_EvadeDown);
                else if (statusReq == Status.shield)
                    Audio.Play(FSPRO.Event.Status_ShieldDown);
                else if (first)
                    Audio.Play(FSPRO.Event.Status_PowerDown);
                c.QueueImmediate(action);
            }
        }
        public override Icon? GetIcon(State s) {
            return (action ?? throw new Exception ("no action set")).GetIcon(s);
        }

        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = (action ?? throw new Exception("no action set")).GetTooltips(s);
            string status = statusReq.ToString();
            if (Manifest.Statuses["DefensiveStance"].Id.ToString()!.Equals(status))
                status = "DefensiveStance";
            else if (Manifest.Statuses["OffensiveStance"].Id.ToString()!.Equals(status))
                status = "OffensiveStance";
            status = string.Concat(status[0].ToString().ToUpper(), status.AsSpan(1));
            list.Add(new TTGlossary(Manifest.Glossary[status + "Cost"]?.Head ??
                throw new Exception("missing glossary entry: status cost hint"), statusCost));
            return list;
        }
    }
}
