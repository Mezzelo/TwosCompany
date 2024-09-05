using Microsoft.Extensions.Logging;
using System;
using TwosCompany.Cards.Sorrel;

namespace TwosCompany.Actions {
    public class AForceAttack : CardAction {
        public int? fromX;
        public int queueAfter = 0;
        public CardAction[]? internalQueue;
        public override void Begin(G g, State s, Combat c) {
            if (!fromX.HasValue)
                fromX = 0;
            if (queueAfter > 0) {
                internalQueue = new CardAction[queueAfter];
                for (int i = 0; i < queueAfter; i++) {
                    internalQueue[i] = c.cardActions[0];
                    c.cardActions.RemoveAt(0);
                }
                queueAfter = 0;
            }
            bool end = false;
            for (int i = 0; i < c.otherShip.parts.Count; i++) {
                end = i == c.otherShip.parts.Count - 1;
                if (c.otherShip.parts[i].intent != null && c.otherShip.parts[i].intent is IntentAttack intent) {
                    timer = 0.0;
                    intent.Apply(s, c, c.otherShip, i);
                    c.otherShip.parts[i].intent = null;
                    if (!end)
                        c.Queue(new AForceAttack() {
                            fromX = i,
                            internalQueue = this.internalQueue,
                            timer = 0.0,
                        });
                    break;
                    /*
                    c.Queue(new AForceAttackApply() {
                       intent = intent,
                       x = i,
                       timer = 0.0,
                    });
                    */
                }
            }
            if (end) {
                if (internalQueue != null && internalQueue.Length > 0) {
                    for (int i = 0; i < internalQueue.Length; i++)
                        c.Queue(internalQueue[i]);
                }
                timer = 0.4;
            }
        }

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconForceAttack"].Id ??
            throw new Exception("missing icon")), null, Colors.status);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["ForceAttack"]?.Head ??
            throw new Exception("missing glossary entry: ForceAttack")) };
    }
}