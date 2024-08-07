﻿using System.Collections.Generic;
using System;
using System.Collections;
using TwosCompany.Midrow;
using Microsoft.Extensions.Logging;

namespace TwosCompany.Actions {
    public class ACascadeAttack : CardAction {

        public int dir = 1;
        public int? originalPos;
        public int damage = 1;
        public int sportsCounter = 0;

        public override void Begin(G g, State s, Combat c) {
            if (originalPos != null && originalPos == s.ship.x)
                return;
            // check for hit
            bool hit = false;
            for (int i = 0; i < s.ship.parts.Count; i++) {
                if (s.ship.parts[i].type == PType.cannon && s.ship.parts[i].active) {
                    RaycastResult ray = CombatUtils.RaycastFromShipLocal(s, c, i, false);
                    if (ray != null && (ray.hitShip || ray.hitDrone)) {
                        if (ray.hitDrone && c.stuff[s.ship.x + i] != null && c.stuff[s.ship.x + i] is FrozenAttack &&
                            !CombatUtils.RaycastGlobal(c, c.otherShip, true, s.ship.x + i).hitShip) {
                            continue;
                        }
                        hit = true;
                        if (ray.hitDrone && c.stuff[s.ship.x + i] != null && c.stuff[s.ship.x + i] is Football)
                            sportsCounter++;
                    }
                }
            }
            c.QueueImmediate(new AAttack() {
                damage = this.damage,
                fast = true,
                targetPlayer = false,
                dialogueSelector = sportsCounter > 5 ? ".mezz_cascadeSports" : null,
            });
            if (sportsCounter > 12 || (c.otherShip.ai != null && c.otherShip.ai is FootballFoe && sportsCounter > 5)) {
                if (sportsCounter < 10)
                    c.Queue(new AAddCard() {
                        card = new YellowCardTrash(),
                        destination = CardDestination.Hand
                    });
            }
            else if (hit) {
                c.Queue(new AMove() {
                    dir = this.dir,
                    targetPlayer = true,
                });
                c.Queue(new ACascadeAttack() {
                    dir = this.dir,
                    originalPos = s.ship.x,
                    timer = 0.0,
                    damage = this.damage,
                    sportsCounter = this.sportsCounter,
                });
            }
        }
    }
}
