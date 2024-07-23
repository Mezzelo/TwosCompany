using Microsoft.Extensions.Logging;
using System;
using TwosCompany.Cards.Sorrel;

namespace TwosCompany.Actions {
    public class AForceAttack : CardAction {
        public int? fromX;
        public bool reverseAfter = false;
        public override void Begin(G g, State s, Combat c) {
            if (!fromX.HasValue)
                fromX = c.otherShip.parts.Count - 1;
            for (int i = fromX.Value; i >= 0; i--) {
                if (reverseAfter) {
                    c.QueueImmediate(new AReverseFrozen() {
                        omitFromTooltips = true,
                    });
                    reverseAfter = false;
                }
                if (c.otherShip.parts[i].intent != null && c.otherShip.parts[i].intent is IntentAttack intent) {
                    c.QueueImmediate(new AForceAttackApply() {
                       intent = intent,
                       x = i,
                       timer = 0.0,
                    });
                }
            }
        }

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconForceAttack"].Id ??
            throw new Exception("missing icon")), null, Colors.status);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["ForceAttack"]?.Head ??
            throw new Exception("missing glossary entry: ForceAttack")) };
    }
}