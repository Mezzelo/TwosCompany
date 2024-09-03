using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System.ComponentModel;
using TwosCompany.Artifacts;
using TwosCompany.Cards.Gauss;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;
using TwosCompany.Midrow;

namespace TwosCompany.Actions {
    public class AChainLightning : AAttack {
        
        /*
        public int damage = 1;
        public int? fromX;
        public bool targetPlayer = false;
        public bool multiCannonVolley;

        public int paybackCounter;
        
        */

        private int? GetFromX(State s, Combat c) {
            if (fromX.HasValue) {
                return fromX;
            }
            int num = (targetPlayer ? c.otherShip : s.ship).parts.FindIndex((Part p) => p.type == PType.missiles && p.active);
            if (num != -1)
                return num;
            else {
                num = (targetPlayer ? c.otherShip : s.ship).parts.FindIndex((Part p) => p.type == PType.missiles && p.active);
            }
            return null;
        }
        private List<int> GetFromXMulti(State s, Combat c) {
            List<int> list = new List<int>();
            for (int i = 0; i < s.ship.parts.Count; i++)
                if (s.ship.parts[i].type == PType.missiles && s.ship.parts[i].active)
                    list.Add(i);
            return list;
        }
        public List<int>? CalculateRoute(int startingPos, bool playerShip, bool hasJump, State s, Combat c) {
            List<int> cRoute = new List<int>();
            if (c.stuff.ContainsKey(startingPos))
                cRoute.Add(startingPos);
            else
                return null;
            bool found = true;
            int dir = -1;
            int search = startingPos;
            while (found) {
                found = true;
                if (c.stuff.ContainsKey(search + dir))
                    search += dir;
                else if (hasJump && c.stuff.ContainsKey(search + dir * 2))
                    search += dir * 2;
                else
                    found = false;
                
                if (found)
                    cRoute.Add(search);
                else if (dir == -1) {
                    found = true;
                    dir = 1;
                    search = startingPos;
                }

            }
            return cRoute;
        }

        public override void Begin(G g, State s, Combat c) {
            timer = 0.0;
            Ship ship = targetPlayer ? s.ship : c.otherShip;
            Ship ship2 = targetPlayer ? c.otherShip : s.ship;
            if (ship == null || ship2 == null || ship.hull <= 0) {
                return;
            }
            int? num = GetFromX(s, c);
            if (num == null) {
                return;
            }
            if (!targetPlayer && g.state.ship.GetPartTypeCount(PType.missiles) > 1 && !multiCannonVolley) {
                c.QueueImmediate(new AVolleyChainLightning {
                    attack = Mutil.DeepCopy(this)
                });
            }
            else {
                Input.Rumble(0.5);
                bool fromElectrocute = !stunEnemy && ship2.Get((Status)Manifest.Statuses?["ElectrocuteCharge"].Id!) > 0;
                bool stunFromCharge = !stunEnemy && ship2.Get(Status.stunCharge) > 0;
                bool piercing = false;
                bool flag = ship2.Get(Status.libra) > 0;
                RaycastResult raycastResult = CombatUtils.RaycastFromShipLocal(s, c, num.Value, targetPlayer);
                bool hasJump = ship2.Get((Status)Manifest.Statuses?["DistantStrike"].Id!) > 0;
                List<int>? cRoute = CalculateRoute(s.ship.x + num.Value, !targetPlayer, hasJump,s, c);

                TwinMaleCable? cable = null;
                List<CardAction> hitActions = new List<CardAction>();

                if (!targetPlayer)
                    foreach (Artifact artif in s.EnumerateAllArtifacts()) {
                        bool? nullable = artif.OnPlayerAttackMakeItPierce(s, c);
                        bool flag2 = true;
                        if (nullable.GetValueOrDefault() == flag2 & nullable.HasValue) {
                            piercing = true;
                            artif.Pulse();
                        }
                        if (artif is FieldResonator) {
                            piercing = true;
                            artif.Pulse();
                        }
                        else if (artif is TwinMaleCable artifCable)
                            cable = artifCable;
                    }
                // no chain
                if (cRoute == null) {
                    hitActions.Add(new AChainHit() {
                        damage = this.damage,
                        targetPlayer = this.targetPlayer,
                        fromX = raycastResult.worldX,
                        piercing = piercing,
                        stun = stunEnemy || fromElectrocute || stunFromCharge,
                        fromDrone = false,
                        timer = this.fast ? 0.1 : 0.4,
                    });
                    /*
                    ChainData.Cannon(g, targetPlayer, CombatUtils.RaycastGlobal(c, ship, false, s.ship.x + num.Value), new DamageDone());
                    foreach (Artifact item10 in s.EnumerateAllArtifacts()) {
                        item10.OnEnemyDodgePlayerAttack(s, c);
                    }
                    bool flag3 = false;
                    for (int i = -1; i <= 1; i += 2) {
                        if (CombatUtils.RaycastGlobal(c, ship, false, raycastResult.worldX + i).hitShip) {
                            flag3 = true;
                            break;
                        }
                    }
                    if (flag3)
                        foreach (Artifact item11 in s.EnumerateAllArtifacts()) {
                            item11.OnEnemyDodgePlayerAttackByOneTile(s, c);
                        }
                    */
                }
                else if (cRoute != null) {
                    raycastResult.hitDrone = true;
                    ChainData.Cannon(g, targetPlayer, raycastResult, new DamageDone());
                    g.state.storyVars.playerJustShotAMidrowObject = true;
                    List<CardAction> actions = new List<CardAction>();
                    List<CardAction> midActions = new List<CardAction>();
                    List<int> alreadyDestroyed = new List<int>();
                    int cDamage = damage;
                    int evadeGain = 0;
                    int shieldGain = 0;
                    bool bubbleDown = false;

                    ExoticMetals? exotics = null;
                    bool hitAsteroid = false;

                    bool salvageArm = s.EnumerateAllArtifacts().OfType<SalvageArm>().Any();
                    TuningTrident? trident = s.EnumerateAllArtifacts().OfType<TuningTrident>().FirstOrDefault();

                    if (!targetPlayer) {
                        exotics = s.EnumerateAllArtifacts().OfType<ExoticMetals>().FirstOrDefault();
                    }

                    for (int n = 0; n < cRoute.Count; n++) {
                        int i = cRoute[n];
                        if (n == 0)
                            c.stuff[i].pulse -= 0.4;
                        // chain ends: mark for fire
                        bool end = n == cRoute.Count - 1 || 
                            (trident != null && n == 0) || (n < cRoute.Count - 1 && cRoute[n] < cRoute[n + 1] && cRoute[n] < cRoute[0]);
                        if (trident != null && cRoute.Count > 0)
                            trident.Pulse();
                        // after left end: reset damage to initial
                        if (n > 0 && cRoute[n] > cRoute[n - 1] && cRoute[n - 1] < cRoute[0]) {
                            if (cable != null)
                                cable.Pulse();
                            else
                                cDamage = 0;
                            cDamage += damage;
                            bubbleDown = false;
                            cDamage += c.stuff[cRoute[0]] is Conduit cond && cond.condType == Conduit.ConduitType.amplifier ? 2 : 1;
                            if (c.stuff[i] is Conduit cond2 && cond2.condType == Conduit.ConduitType.amplifier)
                                cDamage++;
                            // between drone fx
                            ChainData.BetweenDrones(g, cRoute[0], cRoute[n]);
                        } else if (n > 0) {
                            ChainData.BetweenDrones(g, cRoute[n], cRoute[n - 1]);
                        }
                        cDamage++;
                        // damage midrow
                        bool hadBubble = false;
                        if (c.stuff[i].bubbleShield && !bubbleDown) {
                            c.stuff[i].bubbleShield = false;
                            hadBubble = true;
                        }
                        bool invincible = c.stuff[i].Invincible();
                        foreach (Artifact item5 in s.EnumerateAllArtifacts()) {
                            if (item5.ModifyDroneInvincibility(s, c, c.stuff[i]) == true) {
                                invincible = true;
                                item5.Pulse();
                            }
                        }
                        if (invincible) {
                            if (!c.stuff[i].bubbleShield && bubbleDown)
                                c.stuff[i].bubbleShield = true;
                            if (c.stuff[i] is FrozenAttack fAttack && !c.stuff[i].bubbleShield && !hadBubble)
                                fAttack.unfreeze = true;
                            List<CardAction>? invulActions = c.stuff[i].GetActionsOnShotWhileInvincible(s, c, !this.targetPlayer, cDamage - 1);
                            if (invulActions != null) {
                                invulActions.Reverse();
                                midActions.InsertRange(0, invulActions);
                            }
                            if (exotics != null && c.stuff[i] is not Asteroid && c.stuff[i] is not Conduit) {
                                cDamage++;
                                if (!hitAsteroid) {
                                    hitAsteroid = true;
                                    exotics.Pulse();
                                }
                            }
                            List<CardAction>? droneActions = c.stuff[i].GetActions(s, c);
                            if (c.stuff[i] is not Missile && droneActions != null) {
                                midActions.InsertRange(0, droneActions);
                            }
                        }
                        else if (c.stuff[i] is Conduit cond) {
                            if (!c.stuff[i].bubbleShield && bubbleDown)
                                c.stuff[i].bubbleShield = true;
                            if (cond.condType == Conduit.ConduitType.amplifier)
                                cDamage++;
                            else if (cond.condType == Conduit.ConduitType.kinetic && !cond.disabled) {
                                cond.disabled = true;
                                evadeGain += 1;
                            }
                            else if (cond.condType == Conduit.ConduitType.shield && !cond.disabled) {
                                cond.disabled = true;
                                shieldGain += 2;
                                bubbleDown = true;
                            }
                            else if (cond.condType == Conduit.ConduitType.feedback && !cond.disabled) {
                                cond.disabled = true;
                                c.Queue(new AChainLightning() {
                                    damage = 1,
                                    targetPlayer = this.targetPlayer,
                                    dialogueSelector = ".mezz_feedbackProc",
                                });
                                Audio.Play(FSPRO.Event.Status_PowerUp);
                            }
                        } else if (exotics != null && c.stuff[i] is Asteroid) {
                            if (!c.stuff[i].bubbleShield && bubbleDown)
                                c.stuff[i].bubbleShield = true;
                            if (!hitAsteroid) {
                                hitAsteroid = true;
                                exotics.Pulse();
                            }
                        }
                        else {
                            List<CardAction>? droneActions = c.stuff[i].GetActions(s, c);
                            if (exotics != null && c.stuff[i] is not Asteroid) {
                                cDamage++;
                                if (!hitAsteroid) {
                                    hitAsteroid = true;
                                    exotics.Pulse();
                                }
                            }
                            if (c.stuff[i] is not Missile && droneActions != null) {
                                midActions.InsertRange(0, droneActions);
                            }
                            if (bubbleDown && !c.stuff[i].bubbleShield)
                                c.stuff[i].bubbleShield = true;
                            else if (((!hadBubble && !c.stuff[i].bubbleShield) || piercing)) {
                                StuffBase? stuff2 = null;
                                if (i < cRoute[0] && cRoute.Contains(cRoute[0] * 2 - i) &&
                                    !c.stuff[cRoute[0] * 2 - i].Invincible() &&
                                    c.stuff[cRoute[0] * 2 - i] is not Conduit &&
                                    !(exotics != null && c.stuff[cRoute[0] * 2 - i] is Asteroid) &&
                                    (!c.stuff[cRoute[0] * 2 - i].bubbleShield || piercing)) {
                                    stuff2 = c.stuff[cRoute[0] * 2 - i];
                                    alreadyDestroyed.Add(cRoute[0] * 2 - i);
                                }

                                if (!alreadyDestroyed.Contains(i))
                                    actions.Insert(0, new AChainDestroyDrone() {
                                        stuff = c.stuff[i],
                                        stuff2 = stuff2,
                                        playerDidIt = !targetPlayer,
                                        timer = 0.1,
                                        hasSalvageArm = salvageArm,
                                    });
                            }
                        }
                        if (end && !(cable != null && n < cRoute.Count - 1 && n > 0)) {
                            RaycastResult chainRay = CombatUtils.RaycastGlobal(c, ship, true, cRoute[n]);
                            hitActions.Insert(0, new AChainHit() {
                                damage = Math.Max(0, cDamage - (cRoute.Count > 1 && trident != null ? 2 : 0)),
                                targetPlayer = this.targetPlayer,
                                fromX = cRoute[n],
                                piercing = piercing,
                                stun = stunEnemy || fromElectrocute || stunFromCharge,
                                fromDrone = true,
                                timer = (n == cRoute.Count - 1) ? 0.4 : 0.0,
                            });
                        }
                    }
                    Manifest.EventHub.SignalEvent<Tuple<State, int>>("Mezz.TwosCompany.ChainLightning", new(s, cRoute.Count));
                    if (evadeGain > 0)
                        c.QueueImmediate(new AStatus() {
                            targetPlayer = !targetPlayer,
                            status = Status.evade,
                            statusAmount = evadeGain,
                            dialogueSelector = ".mezz_kineticProc",
                        });
                    if (shieldGain > 0)
                        c.QueueImmediate(new AStatus() {
                            targetPlayer = !targetPlayer,
                            status = Status.shield,
                            statusAmount = shieldGain,
                            dialogueSelector = ".mezz_shieldProc"
                        });
                    if (actions.Count > 0) {
                        actions[0].timer = 0.4;
                        foreach (CardAction action in actions)
                            c.QueueImmediate(action);
                    }
                    foreach (CardAction action in midActions)
                        c.QueueImmediate(action);

                    timer = 0.0;
                }
                if (fromElectrocute || stunFromCharge) {
                    if (ship2.Get((Status)Manifest.Statuses?["ElectrocuteCharge"].Id!) > 0) {
                        ship2.Add((Status)Manifest.Statuses?["ElectrocuteCharge"].Id!, -1);
                        ship2.Add((Status)Manifest.Statuses?["ElectrocuteChargeSpent"].Id!, 1);
                        s.ship.PulseStatus((Status)Manifest.Statuses?["ElectrocuteChargeSpent"].Id!);
                    } else if (ship2.Get(Status.stunCharge) > 0)
                        ship2.Set(Status.stunCharge, ship2.Get(Status.stunCharge) - 1);
                }
                if (hasJump) {
                    s.ship.PulseStatus((Status)Manifest.Statuses?["DistantStrike"].Id!);
                    ship2.Add((Status)Manifest.Statuses?["DistantStrike"].Id!, -1);
                }
                if (ship2.Get((Status)Manifest.Statuses?["HeatFeedback"].Id!) > 0) {
                    ship2.PulseStatus((Status)Manifest.Statuses?["HeatFeedback"].Id!);
                    ship2.Add((Status)Manifest.Statuses?["HeatFeedback"].Id!, -1);
                    ship2.Add(Status.heat, 1 + ship2.Get(Status.boost));
                    ship2.Set(Status.boost, 0);
                }
                if (!targetPlayer) {
                    if (flag)
                        DoLibraEffect(c, ship2);

                    foreach (Artifact item4 in s.EnumerateAllArtifacts())
                        item4.OnPlayerAttack(s, c);
                }
                foreach (CardAction action in hitActions)
                    c.QueueImmediate(action);

                Part? partAtLocalX = ship2.GetPartAtLocalX(num.Value);
                if (partAtLocalX != null)
                    partAtLocalX.pulse = 1.0;

                /*
                if (targetPlayer) {
                    if (!raycastResult.hitShip && !raycastResult.hitDrone) {
                        g.state.storyVars.enemyShotJustMissed = true;
                    }
                    if (raycastResult.hitShip) {
                        g.state.storyVars.enemyShotJustHit = true;
                    }
                    foreach (Artifact item6 in s.EnumerateAllArtifacts()) {
                        item6.OnEnemyAttack(s, c);
                    }
                    if (!raycastResult.hitShip && !raycastResult.hitDrone) {
                        foreach (Artifact item7 in s.EnumerateAllArtifacts()) {
                            item7.OnPlayerDodgeHit(s, c);
                        }
                    }
                }
                else {*/
            }
        }

        private bool DoWeHaveCannonsThough(State s) {
            foreach (Part part in s.ship.parts) {
                if (part.type == PType.missiles) {
                    return true;
                }
            }
            if (s.route is Combat combat) {
                foreach (StuffBase value in combat.stuff.Values) {
                    if (value is JupiterDrone) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void DoLibraEffect(Combat c, Ship source) => c.QueueImmediate(new AStatus() {
            targetPlayer = !this.targetPlayer,
            status = Status.tempShield,
            statusAmount = source.Get(Status.libra)
        });

        public override Icon? GetIcon(State s) => 
            this.DoWeHaveCannonsThough(s) ? 
                new Icon((Spr)(Manifest.Sprites["IconChainLightning"].Id!), new int?(this.damage), Colors.redd) : 
                new Icon((Spr)(Manifest.Sprites["IconChainLightning"].Id!), new int?(this.damage), Colors.attackFail);
        public override List<Tooltip> GetTooltips(State s) {
            var list = new List<Tooltip>() {
                new TTGlossary(Manifest.Glossary["ChainLightning"]?.Head ?? throw new Exception("missing glossary entry: chain lightning"), damage),
            };
            if (s.route is Combat c && GetFromX(s, c) != null) {
                if (ChainData.lastShipX != s.ship.x) {
                    ChainData.hilights.Clear();
                    ChainData.lastShipX = s.ship.x;
                }
                ChainData.chainTimer = 2;
                List<int> parts = GetFromXMulti(s, c);
                for (int i = 0; i < parts.Count; i++) {
                    s.ship.GetPartAtLocalX(parts[i])!.hilight = true;
                    bool hasJump = s.ship.Get((Status)Manifest.Statuses?["DistantStrike"].Id!) > 0;
                    List<int>? cRoute = CalculateRoute(s.ship.x + parts[i], !targetPlayer, hasJump, s, c);
                    if (cRoute == null)
                        continue;
                    bool exotics = s.EnumerateAllArtifacts().OfType<ExoticMetals>().Any();
                    bool twin = s.EnumerateAllArtifacts().OfType<TwinMaleCable>().Any();
                    bool trident = s.EnumerateAllArtifacts().OfType<TuningTrident>().Any();
                    int zeroDamage = 0;
                    int cDamage = damage;
                    for (int g = 0; g < cRoute.Count; g++) {
                        if (g > 0 && cRoute[g] > cRoute[g - 1] && cRoute[g - 1] < cRoute[0]) {
                            if (!twin)
                                cDamage = 0;
                            cDamage += zeroDamage;
                        }

                        ++cDamage;
                        if (c.stuff[cRoute[g]] is Conduit cond) {
                            if (cond.condType == Conduit.ConduitType.amplifier)
                                ++cDamage;
                        }
                        else if (exotics && !(c.stuff[cRoute[g]] is Asteroid || c.stuff[cRoute[g]] is Conduit))
                            ++cDamage;
                        
                        if (g == 0)
                            zeroDamage = cDamage;

                        int hilightVal = Math.Max(0, cDamage - (cRoute.Count > 1 && trident ? 2 : 0)) +
                            ((g == cRoute.Count - 1 || 
                            (g == 0 && trident) ||
                            (g < cRoute.Count - 1 && cRoute[g] < cRoute[g + 1] && cRoute[g] < cRoute[0])) ?
                            (twin && g != cRoute.Count - 1 && g > 0 ? 200 : 400) : 0);

                        c.stuff[cRoute[g]].hilight = 2;
                        if (!ChainData.hilights.ContainsKey(cRoute[g]))
                            ChainData.hilights.Add(cRoute[g], hilightVal);
                        else
                            ChainData.hilights[cRoute[g]] = hilightVal;
                    }
                }
            }
            return list;
        }
    }
}
