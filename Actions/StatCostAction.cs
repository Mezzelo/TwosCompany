﻿using System.Collections.Generic;

namespace TwosCompany.Actions {
    public class StatCostAction : CardAction, StatCost {
        public Status statusReq;
        public int statusCost = 1;
        public int cumulative = 0;
        public CardAction? action;
        public bool first = false;
        Status StatCost.statusReq { get => statusReq; }
        int StatCost.statusCost { get => statusCost; }
        int StatCost.cumulative { get => cumulative; }
        CardAction? StatCost.action { get => action; }
        bool StatCost.first { get => first; }
        public override void Begin(G g, State s, Combat c) {
            if (action == null)
                return;
            if (s.ship.statusEffects.ContainsKey(statusReq) && s.ship.statusEffects[statusReq] >= statusCost) {
                s.ship.Set(statusReq, s.ship.Get(statusReq) - this.statusCost);
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
            status = string.Concat(status[0].ToString().ToUpper(), status.AsSpan(1));
            list.Add(new TTGlossary(Manifest.Glossary[status + "Cost"]?.Head ??
                throw new Exception("missing glossary entry: status cost hint"), statusCost));
            return list;
        }
    }
}
