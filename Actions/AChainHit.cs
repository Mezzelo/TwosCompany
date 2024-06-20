using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using TwosCompany.Artifacts;
using TwosCompany.Cards.Gauss;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AChainHit : CardAction {
        public int damage = 1;
        public int fromX;
        public bool targetPlayer = false;
        public bool piercing = false;
        public bool stun = false;
        public bool fromDrone = false;

        public int paybackCounter;

        public override void Begin(G g, State s, Combat c) {
            Ship ship = targetPlayer ? s.ship : c.otherShip;

            if (c.stuff.ContainsKey(fromX))
                c.stuff[fromX].pulse += 0.4;
            RaycastResult raycastResult = CombatUtils.RaycastGlobal(c, ship, fromDrone, fromX);
            DamageDone dmg = new DamageDone();
            bool autododge = ApplyAutododge(c, ship, raycastResult);
            // direct hits
            if (autododge)
                return;
            if (raycastResult.hitShip) {
                if (!targetPlayer && c.otherShip.ai != null) {
                    c.otherShip.ai.OnHitByAttack(s, c, fromX, new AAttack() {
                        damage = this.damage,
                        fromX = this.fromX,
                        piercing = this.piercing,
                        stunEnemy = this.stun,
                    });
                }
                dmg = ship.NormalDamage(s, c, damage, fromX, piercing);
                Part? partAtWorldX = ship.GetPartAtWorldX(fromX);
                bool stunShout = false;

                if (!stun && partAtWorldX != null && partAtWorldX.stunModifier == PStunMod.stunnable) {
                    stunShout = false;
                    stun = true;
                } else if (partAtWorldX == null || (partAtWorldX != null && (partAtWorldX.stunModifier == PStunMod.unstunnable || 
                    partAtWorldX.intent == null))) {
                    stunShout = false;
                }
                if ((ship.Get(Status.payback) > 0 || ship.Get(Status.tempPayback) > 0) && paybackCounter < 100) {
                    c.QueueImmediate(new AAttack {
                        paybackCounter = paybackCounter + 1,
                        damage = Card.GetActualDamage(s, ship.Get(Status.payback) + ship.Get(Status.tempPayback), !targetPlayer),
                        targetPlayer = !targetPlayer,
                        fast = true,
                        storyFromPayback = true,
                        timer = 0.1,
                    });
                }
                if (stun) {
                    c.QueueImmediate(new AStunPart {
                        worldX = fromX,
                        timer = 0.0,
                        dialogueSelector = stunShout ? ".mezz_stunProc" : null,
                    });
                }
                if (ship.Get(Status.reflexiveCoating) >= 1) {
                    c.QueueImmediate(new AArmor {
                        worldX = fromX,
                        targetPlayer = targetPlayer,
                        justTheActiveOverride = true,
                        timer = 0.0,
                    });
                }
                if (!targetPlayer) {
                    g.state.storyVars.playerShotJustHit = true;
                    g.state.storyVars.playerShotJustMissed = false;
                    foreach (Artifact item9 in s.EnumerateAllArtifacts())
                        item9.OnEnemyGetHit(s, c, ship.GetPartAtWorldX(fromX));
                }
            } else {
                if (!targetPlayer && !g.state.storyVars.playerShotJustHit)
                    g.state.storyVars.playerShotJustMissed = true;
                raycastResult.hitShip = false;
                foreach (Artifact item10 in s.EnumerateAllArtifacts())
                    item10.OnEnemyDodgePlayerAttack(s, c);
                for (int i = -1; i <= 1; i += 2) {
                    if (CombatUtils.RaycastGlobal(c, ship, false, raycastResult.worldX + i).hitShip) {
                        foreach (Artifact item11 in s.EnumerateAllArtifacts())
                            item11.OnEnemyDodgePlayerAttackByOneTile(s, c);
                        break;
                    }
                }
            }
            ChainData.Cannon(g, targetPlayer, raycastResult, dmg);
        }
        private bool ApplyAutododge(Combat c, Ship target, RaycastResult ray) {
            if (ray.hitShip) {
                if (target.Get(Status.autododgeRight) > 0) {
                    target.Add(Status.autododgeRight, -1);
                    int dir = ray.worldX - target.x + 1;
                    c.QueueImmediate(new List<CardAction>
                    {
                    new AMove
                    {
                        targetPlayer = targetPlayer,
                        dir = dir,
                        timer = 0.1,
                    },
                    this,
                });
                    timer = 0.0;
                    return true;
                }
                if (target.Get(Status.autododgeLeft) > 0) {
                    target.Add(Status.autododgeLeft, -1);
                    int dir2 = ray.worldX - target.x - target.parts.Count;
                    c.QueueImmediate(new List<CardAction>
                    {
                    new AMove
                    {
                        targetPlayer = targetPlayer,
                        dir = dir2,
                        timer = 0.1,
                    },
                    this,
                });
                    timer = 0.0;
                    return true;
                }
            }
            return false;
        }
    }
}
