
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using daisyowl.text;
using FSPRO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TwosCompany.Actions {
    public class AFollowUp : CardAction {
        public bool dontExhaust = false;
        public override void Begin(G g, State s, Combat c) {
            Card selectedCard = this.selectedCard ?? throw new Exception("no card selected?");
            if (selectedCard == null)
                return;
            if (s.ship.Get((Status)Manifest.Statuses?["FollowUp"].Id!) > 0) {
                bool tryToPlay = c.TryPlayCard(s, selectedCard, true, false);
                if (tryToPlay) {
                    s.ship.PulseStatus((Status)Manifest.Statuses?["FollowUp"].Id!);
                    s.ship.Add((Status)Manifest.Statuses?["FollowUp"].Id!, -1);
                }
                int queueOffset = 0;
                for (int i = 0; i < c.cardActions.Count - queueOffset; i++) {
                    if (c.cardActions[i] is AFollowUp) {
                        c.Queue(c.cardActions[i]);
                        c.cardActions.RemoveAt(i);
                        i--;
                        queueOffset++;
                    }
                }
            }
        }
    }
}