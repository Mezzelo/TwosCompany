using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using daisyowl.text;
using FSPRO;
using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TwosCompany.Actions;
using TwosCompany.Artifacts;
using TwosCompany.Cards;
using TwosCompany.Cards.Gauss;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Jost;
using TwosCompany.Cards.Nola;
using TwosCompany.Helper;
using TwosCompany.ModBG;
using static HarmonyLib.Code;

namespace TwosCompany {
    public class PatchLogic {

        public static bool MoveBegin(AMove __instance, State s, Combat c, out int __state) {
            __state = __instance.targetPlayer ? s.ship.x : c.otherShip.x;
            bool flag = FeatureFlags.Debug && Input.shift;
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (!flag && ship == s.ship && (Enumerable.Any<TrashAnchor>(Enumerable.OfType<TrashAnchor>(c.hand)) ||
                ship.Get(Status.lockdown) > 0 || ship.Get(Status.engineStall) > 0))
                return true;
            ExternalStatus control = Manifest.Statuses?["Control"] ?? throw new Exception("status missing: control");
            if (!flag && ship == s.ship && ship.Get((Status) control.Id!) > 0) {
                Audio.Play(Event.Status_PowerDown);
                ship.shake += 1.0;
                ship.Add(Status.evade, 1);
                ship.Add((Status) control.Id!, -1);
            }

            ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
            if (strafeStatus.Id == null) return true;
            if (ship.Get((Status)strafeStatus.Id) > 0 && (__instance.dir != 0 || (ship.Get(Status.hermes) > 0 && !__instance.ignoreHermes)))
                c.QueueImmediate(new AAttack() {
                    damage = Card.GetActualDamage(s, ship.Get((Status)strafeStatus.Id)),
                    targetPlayer = !__instance.targetPlayer,
                    fast = true,
                    storyFromStrafe = true
                });

            return true;
        }
        public static void MoveEnd(AMove __instance, State s, Combat c, int __state) {
            int dist = (__instance.targetPlayer ? s.ship.x : c.otherShip.x) - __state;
            if (dist != 0) {
                if (__instance.targetPlayer)
                    Manifest.EventHub.SignalEvent<Tuple<int, bool, bool, Combat, State>>(
                        "Mezz.TwosCompany.Movement", new(dist, __instance.targetPlayer, __instance.fromEvade, c, s));

                // this approach sucks, but i can't find where to patch card removal to unhook events
                foreach (Card card in c.hand)
                    if (card is Couch couchCard && __instance.targetPlayer)
                        couchCard.dist += Math.Abs(dist);

                Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
                ExternalStatus mobileStatus = Manifest.Statuses?["MobileDefense"] ?? throw new Exception("status missing: mobile defense");
                if (mobileStatus.Id != null)
                    if (ship.Get((Status)mobileStatus.Id) > 0)
                        c.QueueImmediate(new AStatus() {
                            status = Status.tempShield,
                            statusAmount = ship.Get((Status)mobileStatus.Id),
                            targetPlayer = __instance.targetPlayer,
                            statusPulse = (Status)mobileStatus.Id,
                        });

                ExternalStatus fortress = Manifest.Statuses?["Fortress"] ?? throw new Exception("status missing: fortress");
                if (fortress.Id != null)
                    if (ship.Get((Status)fortress.Id) > 0)
                        c.QueueImmediate(new AStatus() {
                            status = (Status)fortress.Id,
                            statusAmount = -1,
                            targetPlayer = __instance.targetPlayer,
                            statusPulse = (Status)fortress.Id,
                        });
            }
            return;
        }

        public struct healthStats {
            public healthStats(int hull, int shield, int tempShield, int autoDodge) {
                this.hull = hull;
                this.shield = shield;
                this.tempShield = tempShield;
                this.autoDodge = autoDodge;
            }
            public int hull { get; }
            public int shield { get; }
            public int tempShield { get; }
            public int autoDodge { get; }
        }
        public static bool AttackBegin(AAttack __instance, State s, Combat c, out healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            __state = new healthStats(ship.hull, ship.Get(Status.shield), ship.Get(Status.tempShield),
                ship.Get(Status.autododgeLeft) + ship.Get(Status.autododgeRight));

            return true;
        }
        public static void AttackEnd(AAttack __instance, State s, Combat c, healthStats __state) {
            Ship ship = __instance.targetPlayer ? c.otherShip : s.ship;
            Ship target = __instance.targetPlayer ? s.ship : c.otherShip;
            if (!__instance.fromDroneX.HasValue && ship.Get((Status)Manifest.Statuses?["HeatFeedback"].Id!) > 0) {
                ship.PulseStatus((Status)Manifest.Statuses?["HeatFeedback"].Id!);
                ship.Add((Status)Manifest.Statuses?["HeatFeedback"].Id!, -1);
                ship.Add(Status.heat, 1);
            }
            if (target.hull < __state.hull || target.Get(Status.shield) < __state.shield || target.Get(Status.tempShield) < __state.tempShield) {
                ExternalStatus falseStatus = Manifest.Statuses?["FalseOpening"] ?? throw new Exception("status missing: falseopening");
                if (falseStatus.Id != null) {
                    if (target.Get((Status)falseStatus.Id) > 0) {
                        c.QueueImmediate(new AStatus() {
                            status = Status.overdrive,
                            mode = AStatusMode.Add,
                            statusAmount = 1,
                            targetPlayer = __instance.targetPlayer,
                            statusPulse = (Status) falseStatus.Id,
                        });
                        c.QueueImmediate(new AStatus() {
                            status = (Status) falseStatus.Id,
                            mode = AStatusMode.Add,
                            statusAmount = -1,
                            targetPlayer = __instance.targetPlayer,
                            timer = 0.0,
                        });
                    }
                    ExternalStatus falseBStatus = Manifest.Statuses?["FalseOpeningB"] ?? throw new Exception("status missing: falseopeningb");
                    if (falseBStatus.Id != null) {
                        if (target.Get((Status)falseBStatus.Id) > 0) {
                            c.QueueImmediate(new AStatus() {
                                status = Status.powerdrive,
                                statusAmount = 1,
                                targetPlayer = __instance.targetPlayer,
                                statusPulse = (Status)falseBStatus.Id,
                            });
                            c.QueueImmediate(new AStatus() {
                                status = (Status)falseBStatus.Id,
                                statusAmount = -1,
                                targetPlayer = __instance.targetPlayer,
                                timer = 0.0,
                            });
                        }
                    }
                }
            }
            return;
        }

        public static bool MissileHitBegin(AMissileHit __instance, State s, Combat c, out healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            __state = new healthStats(ship.hull, ship.Get(Status.shield), ship.Get(Status.tempShield), 
                ship.Get(Status.autododgeLeft) + ship.Get(Status.autododgeRight));

            return true;
        }
        public static void MissileHitEnd(AMissileHit __instance, State s, Combat c, healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (ship.hull < __state.hull || ship.Get(Status.shield) < __state.shield || ship.Get(Status.tempShield) < __state.tempShield) {
                ExternalStatus falseStatus = Manifest.Statuses?["FalseOpening"] ?? throw new Exception("status missing: falseopening");
                if (ship.Get((Status) falseStatus.Id!) > 0) {
                    c.QueueImmediate(new AStatus() {
                        status = Status.overdrive,
                        mode = AStatusMode.Add,
                        statusAmount = 1,
                        targetPlayer = __instance.targetPlayer,
                        statusPulse = (Status)falseStatus.Id,
                    });
                    c.QueueImmediate(new AStatus() {
                        status = (Status)falseStatus.Id,
                        mode = AStatusMode.Add,
                        statusAmount = -1,
                        targetPlayer = __instance.targetPlayer,
                        timer = 0.0,
                    });
                }
                ExternalStatus falseBStatus = Manifest.Statuses?["FalseOpeningB"] ?? throw new Exception("status missing: falseopeningb");
                if (falseBStatus.Id != null) {
                    if (ship.Get((Status)falseBStatus.Id) > 0) {
                        c.QueueImmediate(new AStatus() {
                            status = Status.powerdrive,
                            statusAmount = 1,
                            targetPlayer = __instance.targetPlayer,
                            statusPulse = (Status)falseBStatus.Id,
                        });
                        c.QueueImmediate(new AStatus() {
                            status = (Status)falseBStatus.Id,
                            statusAmount = -1,
                            targetPlayer = __instance.targetPlayer,
                            timer = 0.0,
                        });
                    }
                }
            }
            return;
        }

        public static void TurnEndStance(CrumpledWrit? artif, State state, Combat combat) {
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            ExternalStatus standFirm = Manifest.Statuses?["StandFirm"] ?? throw new Exception("status missing: standfirm");
            if (state.ship.Get((Status)defensiveStance.Id!) + state.ship.Get((Status)offensiveStance.Id!) > 1 &&
                state.ship.Get((Status)standFirm.Id!) <= 0 && state.ship.Get(Status.timeStop) <= 0) {

                state.ship.Add((Status)defensiveStance.Id, -1);
                if (state.ship.Get((Status)offensiveStance.Id!) > 0)
                    state.ship.Add((Status)offensiveStance.Id, -1);
                Audio.Play(FSPRO.Event.Status_ShieldDown);
            }
        }

        public static bool TurnBegin(Ship __instance, State s, Combat c) {
            if (__instance.Get((Status)Manifest.Statuses["Footwork"].Id!) > 0) {
                if (Stance.Get(s) == 2)
                    c.QueueImmediate(new ADummyAction() {
                        timer = 0.0,
                        dialogueSelector = ".mezz_recklessAbandon",
                    });
                __instance.Set((Status)Manifest.Statuses["DefensiveStance"].Id!, __instance.Get((Status)Manifest.Statuses["DefensiveStance"].Id!) + 1);
                Audio.Play(FSPRO.Event.Status_ShieldUp);
                if (__instance.Get(Status.timeStop) <= 0)
                    __instance.Set((Status)Manifest.Statuses["Footwork"].Id!, __instance.Get((Status)Manifest.Statuses?["Footwork"].Id!) - 1);
            }

            if (__instance.Get(Status.timeStop) <= 0) {
                if (__instance.Get((Status)Manifest.Statuses["TempStrafe"].Id!) > 0)
                    __instance.Set((Status)Manifest.Statuses["TempStrafe"].Id!, 0);
                if (__instance.Get((Status)Manifest.Statuses["FalseOpening"].Id!) > 0)
                    __instance.Set((Status)Manifest.Statuses["FalseOpening"].Id!, 0);
                if (__instance.Get((Status)Manifest.Statuses["FalseOpeningB"].Id!) > 0)
                    __instance.Set((Status)Manifest.Statuses["FalseOpeningB"].Id!, 0);
                if (__instance.Get((Status)Manifest.Statuses["StandFirm"].Id!) > 0)
                    __instance.Add((Status)Manifest.Statuses["StandFirm"].Id!, -1);

            }
            if (__instance.Get((Status)Manifest.Statuses?["Enflamed"].Id!) > 0) {
                c.QueueImmediate(new AStatus() {
                    status = Status.heat,
                    statusAmount = __instance.Get((Status)Manifest.Statuses?["Enflamed"].Id!),
                    targetPlayer = __instance.isPlayerShip,
                    statusPulse = (Status)Manifest.Statuses?["Enflamed"].Id!,
                });
            }
            if (__instance.Get((Status)Manifest.Statuses["ElectrocuteChargeSpent"].Id!) > 0) {
                __instance.Set((Status)Manifest.Statuses["ElectrocuteCharge"].Id!, __instance.Get((Status)Manifest.Statuses["ElectrocuteCharge"].Id!) +
                    __instance.Get((Status)Manifest.Statuses["ElectrocuteChargeSpent"].Id!));
                __instance.Set((Status)Manifest.Statuses["ElectrocuteChargeSpent"].Id!, 0);
            }

            ExternalStatus fortress = Manifest.Statuses?["Fortress"] ?? throw new Exception("status missing: fortress");
            if (fortress.Id != null)
                if (__instance.Get((Status)fortress.Id) > 0) {
                    c.QueueImmediate(new AStatus() {
                        status = Status.tempShield,
                        statusAmount = __instance.Get((Status)fortress.Id),
                        targetPlayer = true,
                        statusPulse = (Status)fortress.Id,
                    });
                    if (__instance.Get((Status)fortress.Id) > 1)
                        c.QueueImmediate(new AStatus() {
                            status = Status.engineStall,
                            statusAmount = __instance.Get((Status)fortress.Id) - 1,
                            targetPlayer = true,
                            statusPulse = (Status)fortress.Id,
                        });
                }

            if (__instance.isPlayerShip) {
                if (s.ship.Get((Status)Manifest.Statuses["HyperspaceStorm"].Id!) > 0)
                    c.QueueImmediate(new AAddCard() {
                        card = new HyperspaceWind(),
                        destination = CardDestination.Hand,
                        amount = 1,
                        statusPulse = (Status)Manifest.Statuses["HyperspaceStorm"].Id!,
                        timer = 0.4,
                    });
                if (s.ship.Get((Status)Manifest.Statuses["HyperspaceStormA"].Id!) > 0)
                    c.QueueImmediate(new AAddCard() {
                        card = new HyperspaceWind() { upgrade = Upgrade.A },
                        destination = CardDestination.Hand,
                        amount = 1,
                        statusPulse = (Status)Manifest.Statuses["HyperspaceStormA"].Id!,
                        timer = 0.4,
                    });
                if (s.ship.Get((Status)Manifest.Statuses["HyperspaceStormB"].Id!) > 0)
                    c.QueueImmediate(new AAddCard() {
                        card = new HyperspaceWind() { upgrade = Upgrade.B },
                        destination = CardDestination.Hand,
                        amount = 1,
                        statusPulse = (Status)Manifest.Statuses["HyperspaceStormB"].Id!,
                        timer = 0.4,
                    });
            }

            return true;
        }

        public static void TurnEnd(Ship __instance, State s, Combat c) {
            if (__instance.isPlayerShip) {
                foreach (Card card in c.hand)
                    if (card is ITurnIncreaseCard increaseCard) {
                        card.discount += increaseCard.increasePerTurn;
                        increaseCard.costIncrease += increaseCard.increasePerTurn;
                    }

                if (__instance.Get((Status)Manifest.Statuses["Autocurrent"].Id!) > 0)
                    for (int i = 0; i < __instance.Get((Status)Manifest.Statuses["Autocurrent"].Id!); i++)
                        c.Queue(new AChainLightning() {
                            damage = Card.GetActualDamage(s, 1, !__instance.isPlayerShip),
                            targetPlayer = false,
                        });
            }

            ExternalStatus dodgeStatus = Manifest.Statuses?["UncannyEvasion"] ?? throw new Exception("status missing: uncanny evasion");
            if (dodgeStatus.Id == null) return;
            if (__instance.Get((Status)dodgeStatus.Id) > 0 && __instance.Get(Status.shield) <= 0)
                c.QueueImmediate(new AStatus() {
                    status = Status.autododgeRight,
                    statusAmount = __instance.Get((Status)dodgeStatus.Id),
                    targetPlayer = __instance.isPlayerShip,
                    statusPulse = (Status)dodgeStatus.Id,
                });

            if (__instance.Get(Status.timeStop) <= 0) {
                if (__instance.Get((Status)Manifest.Statuses["MobileDefense"].Id!) > 0)
                    c.QueueImmediate(new AStatus {
                        status = (Status)Manifest.Statuses["MobileDefense"].Id!,
                        mode = AStatusMode.Add,
                        statusAmount = -1,
                        targetPlayer = __instance.isPlayerShip
                    });
                if (__instance.Get((Status)Manifest.Statuses["Onslaught"].Id!) > 0)
                    __instance.Set((Status)Manifest.Statuses["Onslaught"].Id!, 0);
                if (__instance.Get((Status)Manifest.Statuses["Superposition"].Id!) > 0)
                    c.QueueImmediate(new AStatus {
                        status = (Status)Manifest.Statuses["Superposition"].Id!,
                        statusAmount = -1,
                        targetPlayer = __instance.isPlayerShip
                    });
            }

            if (__instance.isPlayerShip) {
                if (s.ship.Get((Status)Manifest.Statuses["BattleTempo"].Id!) > 0)
                    c.QueueImmediate(new AAddCard() {
                        card = new Heartbeat() { temporaryOverride = true, exhaustOverride = true, },
                        destination = CardDestination.Deck,
                        amount = s.ship.Get((Status)Manifest.Statuses["BattleTempo"].Id!),
                        handPosition = 0,
                        insertRandomly = false,
                        statusPulse = (Status)Manifest.Statuses["BattleTempo"].Id!,
                        timer = 0.2,
                    });
                TurnEndStance(null, s, c);
            }
        }

        public static void PlayCardPrefix(Combat __instance, State s, Card card, bool playNoMatterWhatForFree, bool exhaustNoMatterWhat) {
            ChainData.chainTimer = 0;
            ChainData.hilights.Clear();
        }

        public static void PlayCardPostfix(Combat __instance, ref bool __result, State s, Card card, bool playNoMatterWhatForFree, bool exhaustNoMatterWhat) {
            if (!__result)
                return;
            if (card is IJostCard)
                __instance.Queue(new AStanceSwitch());
            if (s.route is Combat c) {
                List<CardAction> actions = card.GetActions(s, c);
                bool isAttack = false;
                foreach (CardAction action in actions) {
                    if (action is AAttack && !action.disabled) {
                        isAttack = true;
                        break;
                    }
                }
                if (isAttack) {
                    List<Card> cards = c.hand;
                    foreach (Card searchCard in cards) {
                        if (searchCard is IOtherAttackIncreaseCard oic && searchCard.uuid != card.uuid)
                            oic.OtherAttackDiscount(s);
                        else if (searchCard is MoveAsOne mao && searchCard.uuid != card.uuid &&
                            Stance.Get(s) % 2 == 1)
                            mao.OtherAttackDiscount(s);
                    }
                }
            }
            ExternalStatus onslaughtStatus = Manifest.Statuses?["Onslaught"] ?? throw new Exception("status missing: onslaught");
            if (onslaughtStatus.Id == null) return;
            if (s.ship.Get((Status)onslaughtStatus.Id) > 0) {
                bool fromDiscard = false;
                bool shuffle = false;
                if (__instance.discard.Count > 0) {
                    for (int i = __instance.discard.Count - 1; i >= 0; --i)
                        if (__instance.discard[i].GetMeta().deck == card.GetMeta().deck)
                            if (card.uuid != __instance.discard[i].uuid) {
                                fromDiscard = true;
                                shuffle = true;
                                break;
                            }
                }
                //  && count < s.ship.statusEffects[(Status)onslaughtStatus.Id]
                for (int drawIdx = s.deck.Count - 1; drawIdx >= 0; --drawIdx) {
                    Card selectCard = s.deck[drawIdx];
                    if (selectCard.GetMeta().deck == card.GetMeta().deck) {
                        if (card.uuid != selectCard.uuid) {
                            fromDiscard = false;
                            if (__instance.hand.Count >= 10) {
                                __instance.PulseFullHandWarning();
                                break;
                            }
                            __instance.DrawCardIdx(s, drawIdx, fromDiscard ? CardDestination.Discard : CardDestination.Deck);
                            s.ship.PulseStatus((Status)onslaughtStatus.Id);
                            Audio.Play(FSPRO.Event.CardHandling);
                            s.ship.Add((Status)onslaughtStatus.Id, -1);
                            foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts())
                                enumerateAllArtifact.OnDrawCard(s, __instance, 1);
                            // continue;
                            break;
                        }
                    }
                }

                if (fromDiscard) {
                    for (int drawIdx = __instance.discard.Count - 1; drawIdx >= 0; --drawIdx) {
                        Card selectCard = __instance.discard[drawIdx];
                        if (selectCard.GetMeta().deck == card.GetMeta().deck) {
                            if (card.uuid != selectCard.uuid) {
                                if (__instance.hand.Count >= 10) {
                                    __instance.PulseFullHandWarning();
                                    shuffle = false;
                                    break;
                                }
                                __instance.DrawCardIdx(s, drawIdx, fromDiscard ? CardDestination.Discard : CardDestination.Deck);
                                s.ship.PulseStatus((Status)onslaughtStatus.Id);
                                Audio.Play(FSPRO.Event.CardHandling);
                                s.ship.Add((Status)onslaughtStatus.Id, -1);
                                foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts())
                                    enumerateAllArtifact.OnDrawCard(s, __instance, 1);
                                // continue;
                                break;
                            }
                        }
                    }
                    if (shuffle) {
                        foreach (Card thisCard in __instance.discard)
                            s.SendCardToDeck(thisCard, true, true);
                        __instance.discard.Clear();
                        s.ShuffleDeck(true);
                    }
                }
            }
        }

        public static void CardDataPostfix(Card __instance, ref CardData __result, State state) {
            if (state.route is not Combat)
                return;
            if (__instance is IJostCard && !__result.flippable && 
                !__result.floppable && state.ship.Get((Status)Manifest.Statuses["Superposition"].Id!) > 0 &&
                state.ship.Get((Status)Manifest.Statuses["DefensiveStance"].Id!) != state.ship.Get((Status)Manifest.Statuses["OffensiveStance"].Id!))
                __result.floppable = true;
            /*
            if (!__result.flippable && state.ship.Get(Status.tableFlip) > 0) {
                if (__instance is Wildfire ||
                __instance is PointDefense ||
                __instance is AllHands ||
                __instance is Cascade) {
                    __result.flippable = true;
                }
            }*/
        }

        public static void ResetHilightsPrefix(Combat __instance) {
            if (ChainData.chainTimer > 0) {
                ChainData.chainTimer--;
                if (ChainData.chainTimer == 0)
                    ChainData.hilights.Clear();
            }
        }

        public static void ADroneMovePrefix(ADroneMove __instance, G g, State s, Combat c) {
            if (__instance.dir != 0) {
                ChainData.chainTimer = 0;
                ChainData.hilights.Clear();
            }

        }

        public static void RenderHintsUnderlayPrefix(Combat __instance, G g) {

            g.Push(rect: default(Rect) + Combat.arenaPos + __instance.GetCamOffset());
            foreach (StuffBase value in __instance.stuff.Values) {
                if (!ChainData.hilights.ContainsKey(value.x))
                    continue;
                int x = Math.Abs(ChainData.hilights[value.x]);
                if (x < 300)
                    continue;
                Box box = g.Push(rect: value.GetGetRect());
                Vec v2 = box.rect.xy;
                bool targetPlayer = x < 0;
                Color color2 = targetPlayer ? Colors.redd : Colors.attackStatusHintPlayer;
                Ship shipTarget = targetPlayer ? g.state.ship : __instance.otherShip;
                Ship shipSource = targetPlayer ? __instance.otherShip : g.state.ship;
                double y = shipTarget.HasNonEmptyPartAtWorldX(value.x) ? 50.0 : 300.0;
                Draw.Rect(v2.x, v2.y - y + 14.0, 15.0, y, color2, BlendMode.Screen);

                g.Pop();
            }
            g.Pop();

        }

        public static void DestroyDroneAtPrefix() {
            ChainData.chainTimer = 0;
            ChainData.hilights.Clear();
        }

        public static void DrawLightningNumbers(int x, G g, Vec v) {
            if (ChainData.chainTimer > 0 && ChainData.hilights.ContainsKey(x)) {
                // Vec v2 = v - sb.GetOffset(g, false);
                // v2.x = v2.x - v2.x % 1.0;
                // v2.y = v2.y - v2.y % 1.0;
                int endType = Math.Abs(ChainData.hilights[x]);
                bool targetPlayer = ChainData.hilights[x] < 0;
                int damage = endType % 100;
                Color drawColor = Colors.droneOutline;
                if (endType >= 400)
                    drawColor = Colors.redd;
                else if (endType >= 200)
                    drawColor = Colors.attackFail;
                if (damage > 9) {
                    if (endType >= 400)
                        BigNumbers.Render(damage, v.x + 2.0 + 1.0, v.y + 25.0, drawColor);
                    BigNumbers.Render(damage, v.x + 2.0, v.y + 25.0, drawColor);
                }
                else {
                    if (endType >= 400)
                        BigNumbers.Render(damage, v.x + 5.0 + 1.0, v.y + 25.0, drawColor);
                    BigNumbers.Render(damage, v.x + 5.0, v.y + 25.0, drawColor);
                }
            }
        }

        public static void RenderDronesPrefix(Combat __instance, G g) {
            if (ChainData.chainTimer <= 0)
                return;
            g.Push(null, default(Rect) + Combat.arenaPos + __instance.GetCamOffset());
            foreach (int x in ChainData.hilights.Keys) {
                if (__instance.stuff.ContainsKey(x)) {
                    Box box = g.Push(rect: new Rect?(__instance.stuff[x].GetGetRect()));
                    DrawLightningNumbers(x, g, box.rect.xy);
                    g.Pop();
                }
            }
            g.Pop();
        }

        public static bool Card_StatCostAction_Prefix(G g, State state, ref CardAction action, bool dontDraw) {
            int iconWidth = 6;
            Spr? id = null;
            Spr? idNotMet = null;

            Status statusReq;
            int statusCost = 0;
            int cumulative = 0;

            if (dontDraw)
                return true;

            if (action is IStatCost cast) {
                statusReq = cast.statusReq;
                statusCost = cast.statusCost;
                cumulative = cast.cumulative;
                ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
                ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
                if (cast.statusReq == Status.evade) {
                    id = (Spr)(Manifest.Sprites["IconEvadeCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconEvadeCostOff"].Id ?? throw new Exception("missing icon"));
                } else if (cast.statusReq == Status.shield) {
                    id = (Spr)(Manifest.Sprites["IconShieldCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconShieldCostOff"].Id ?? throw new Exception("missing icon"));
                } else if (cast.statusReq == Status.heat) {
                    id = (Spr)(Manifest.Sprites["IconHeatCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconHeatCostOff"].Id ?? throw new Exception("missing icon"));
                } else if (cast.statusReq == (Status)defensiveStance.Id!) {
                    id = (Spr)(Manifest.Sprites["IconDefensiveStanceCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconDefensiveStanceCostOff"].Id ?? throw new Exception("missing icon"));
                } else if (cast.statusReq == (Status)offensiveStance.Id!) {
                    id = (Spr)(Manifest.Sprites["IconOffensiveStanceCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconOffensiveStanceCostOff"].Id ?? throw new Exception("missing icon"));
                }
                else
                    return true;
            } else
                return true;

            int w = -4 - ((iconWidth - 2) * statusCost);

            for (int i = 0; i < statusCost; i++) {
                bool metCost = false;
                if (state.route is Combat && state.ship.statusEffects.ContainsKey(statusReq))
                    metCost = state.ship.Get(statusReq) >= i + cumulative + 1;
                --w;
                var nullable1 = new Rect(w);
                var key = new UIKey?();
                var rect = nullable1;
                var rectForReticle = new Rect?();
                var rightHint = new UIKey?();
                var leftHint = new UIKey?();
                var upHint = new UIKey?();
                var downHint = new UIKey?();
                var xy = g.Push(key, rect, rectForReticle, rightHint: rightHint, leftHint: leftHint, upHint: upHint, downHint: downHint).rect.xy;
                double x = xy.x;
                double y = xy.y;
                Color? nullable2 = action.disabled ? Colors.disabledIconTint : new Color("ffffff");
                Vec? originPx = new Vec?();
                Vec? originRel = new Vec?();
                Vec? scale = new Vec?();
                Rect? pixelRect = new Rect?();
                Color? color = nullable2;
                Draw.Sprite(metCost ? id : idNotMet
                    , x, y, originPx: originPx, originRel: originRel, scale: scale, pixelRect: pixelRect, color: color);
                g.Pop();
                w += iconWidth - 1;
            }

            return true;
        }

        public static void Card_OnDraw_Postfix(State s, Card card, Combat __instance) {
            if (s.ship.Get((Status)Manifest.Statuses?["FollowUp"].Id!) > 0) {
                __instance.Queue(new AFollowUp() {
                    selectedCard = card,
                });
            }
        }
        /* i have decided this artifact was too funny.
        public static bool Card_DrainCardActions_Prefix(Combat __instance, G g) {
            if (__instance.cardActions.Count <= 0)
                return true;
            string key = "";
            if (g.state.EnumerateAllArtifacts().OfType<ManualSteering>().FirstOrDefault() == null)
                return true;
            else
                key = g.state.EnumerateAllArtifacts().OfType<ManualSteering>().FirstOrDefault()!.Key();
            if (__instance.cardActions[0] is AAttack a && !a.storyFromStrafe && 
                (__instance.cardActions.Count == 1 || __instance.cardActions[1] is not AIsaMarker)) {
                __instance.cardActions.Insert(1, new AIsaMarker());
                int missByOne = 0;
                for (int f = 0; f < g.state.ship.parts.Count && missByOne == 0; f++) {
                    if (g.state.ship.parts[f].type == PType.cannon && g.state.ship.parts[f].active) {
                        RaycastResult raycastResult = CombatUtils.RaycastFromShipLocal(g.state, __instance, f, false);
                        if (!raycastResult.hitShip && !raycastResult.hitDrone) {
                            for (int n = -1; n <= 1; n += 2) {
                                if (CombatUtils.RaycastGlobal(__instance, __instance.otherShip, fromDrone: true, raycastResult.worldX + n).hitShip) {
                                    missByOne = g.state.ship.x + f < __instance.otherShip.x + __instance.otherShip.parts.Count / 2 ? 1 : -1;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (missByOne != 0)
                    __instance.cardActions.Insert(0, new AMove() { targetPlayer = true, fromEvade = false, dir = missByOne, artifactPulse = key });
            }
            return true;
        }
        public static void Card_BeginCardAction_Postfix(Combat __instance, G g) {
            if (__instance.cardActions.Count <= 0)
                return;
            string key = "";
            if (g.state.EnumerateAllArtifacts().OfType<ManualSteering>().FirstOrDefault() == null)
                return;
            else
                key = g.state.EnumerateAllArtifacts().OfType<ManualSteering>().FirstOrDefault()!.Key();
            if (__instance.cardActions[0] is AAttack a && !a.storyFromStrafe && 
                (__instance.cardActions.Count == 1 || __instance.cardActions[1] is not AIsaMarker)) {
                __instance.cardActions.Insert(1, new AIsaMarker());
                int missByOne = 0;
                for (int f = 0; f < g.state.ship.parts.Count && missByOne == 0; f++) {
                    if (g.state.ship.parts[f].type == PType.cannon && g.state.ship.parts[f].active) {
                        RaycastResult raycastResult = CombatUtils.RaycastFromShipLocal(g.state, __instance, f, false);
                        if (!raycastResult.hitShip && !raycastResult.hitDrone) {
                            for (int n = -1; n <= 1; n += 2) {
                                if (CombatUtils.RaycastGlobal(__instance, __instance.otherShip, fromDrone: true, raycastResult.worldX + n).hitShip) {
                                    missByOne = g.state.ship.x + f < __instance.otherShip.x + __instance.otherShip.parts.Count / 2 ? 1 : -1;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (missByOne != 0)
                    __instance.cardActions.Insert(0, new AMove() { targetPlayer = true, fromEvade = false, dir = missByOne, artifactPulse = key });
            }
        }
        */
        public static void Card_FlipCardInHand_Postfix(G g, Card card) {
            if (card is IJostCard && g.state.route is Combat &&
                g.state.ship.Get((Status)Manifest.Statuses["Superposition"].Id!) > 0 &&
                g.state.ship.Get((Status)Manifest.Statuses["OffensiveStance"].Id!) - 
                    g.state.ship.Get((Status)Manifest.Statuses["DefensiveStance"].Id!) != 0) {
                if (card is IJostFlippableCard jCard && card.GetDataWithOverrides(g.state).flippable && !jCard.markForFlop) {
                    jCard.markForFlop = true;
                    card.flopAnim = 0.0;
                    card.flipAnim = 1.0;
                } else {
                    int off = g.state.ship.Get((Status)Manifest.Statuses["OffensiveStance"].Id!);
                    g.state.ship.Set((Status)Manifest.Statuses["OffensiveStance"].Id!, g.state.ship.Get((Status)Manifest.Statuses["DefensiveStance"].Id!));
                    g.state.ship.Set((Status)Manifest.Statuses["DefensiveStance"].Id!, off);
                    if (card.GetDataWithOverrides(g.state).floppable || card.GetDataWithOverrides(g.state).flippable)
                        card.flipped = !card.flipped;
                    if (card is IJostFlippableCard jCard2 && card.GetDataWithOverrides(g.state).flippable) {
                        jCard2.markForFlop = !jCard2.markForFlop;
                        card.flipAnim = 0.0;
                    }
                    card.flopAnim = g.state.ship.Get((Status)Manifest.Statuses["OffensiveStance"].Id!) >
                        g.state.ship.Get((Status)Manifest.Statuses["DefensiveStance"].Id!) ? -1.0 : 1.0;
                }

            }
        }

        public static void Card_GetActualDamage_Postfix(State s, Card __instance, bool targetPlayer, ref int __result) {
            if (s.route is Combat route) {
                Ship ship = targetPlayer ? route.otherShip : s.ship;
                __result += ship.Get((Status) Manifest.Statuses?["HeatFeedback"].Id!) > 0 ? 1 : 0;
            } else 
                return;
        }

        public static bool DisguisedCardName(Card __instance, ref string __result) {
            if (__instance is IDisguisedCard) {
                if (((IDisguisedCard)__instance).disguised) {
                    String b;
                    switch (__instance.upgrade) {
                        case Upgrade.None:
                            b = "";
                            break;
                        case Upgrade.A:
                            b = " A";
                            break;
                        case Upgrade.B:
                            b = " B";
                            break;
                        default:
                            b = " ?";
                            break;
                    }
                    __result = __instance.Name() + b;
                    return false;
                }
            }
            return true;
        }

        public static void LocalePostfix(ref Dictionary<string, string> __result, string locale) {
            if (locale != "en")
                return;
            __result.Add("char." + ManifHelper.GetDeckId("nola") + ".desc.locked",
                Manifest.NolaColH + "NOLA</c>\nWin a run with at least <c=keyword>30</c> cards in your deck <c=keyword>OR</c> " +
                "with both " + Manifest.IsaColH + "Isabelle</c> and " + Manifest.IlyaColH + "Ilya</c> to unlock " + Manifest.NolaColH + "Nola</c>" + "!");
            __result.Add("char." + ManifHelper.GetDeckId("isa") + ".desc.locked",
                Manifest.IsaColH + "ISABELLE</c>\nDefeat a boss <c=downside>with full hull remaining</c> <c=keyword>OR</c> " +
                "beat a run with <c=peri>Peri</c> on <c=downside>HARD</c> or harder to unlock " + Manifest.IsaColH + "Isabelle</c>" + "!");
            __result.Add("char." + ManifHelper.GetDeckId("ilya") + ".desc.locked",
                Manifest.IlyaColH + "ILYA</c>\nReach 7 <c=status>HEAT</c> <c=keyword>OR</c> " +
                "beat a run with <c=eunice>Drake</c> on <c=downside>HARD</c> or harder to unlock " + Manifest.IlyaColH + "Ilya</c>" + "!");
            __result.Add("char." + ManifHelper.GetDeckId("jost") + ".desc.locked",
                Manifest.JostColH + "JOST</c>\nDefeat a boss <c=downside>without moving</c> <c=keyword>OR</c> " +
                "beat a run with " + Manifest.IsaColH + "Isabelle</c> on <c=downside>HARD</c> or harder to unlock " + Manifest.JostColH + "Jost</c>" + "!");
            __result.Add("char." + ManifHelper.GetDeckId("gauss") + ".desc.locked",
                Manifest.GaussColH + "GAUSS</c>\nDestroy <c=keyword>10</c> midrow objects in a single encounter <c=keyword>OR</c> " +
                "beat a run with <c=goat>Isaac</c> on <c=downside>HARD</c> or harder to unlock " + Manifest.GaussColH + "Gauss</c>" + "!");
        }

        // this small bit of loading logic courtesy of shockah's dialogue implementation for soggins.
        public static void DialogueInjectionPrefix(MG __instance, ref int __state) => __state = __instance.loadingQueue?.Count ?? 0;

        public static void DialogueInjectionPostfix(MG __instance, ref int __state) {
            if (__state <= 0 || (__instance.loadingQueue?.Count ?? 0) > 0)
                return;
            Manifest.PatchStory("story_inject", Mutil.LoadJsonFile<Dictionary<string, string>>(
                Path.Combine(Manifest.Instance.ModRootFolder!.FullName, "locales", Path.GetFileName("inject_en.json"))));
        }

        public static bool AICombatStartPrefix(AI __instance, State s, Combat c) {
            if (Enumerable.Any(s.characters, ch => {
                Deck? deckType = ch.deckType;
                return (int)deckType.GetValueOrDefault() == Manifest.IlyaDeck!.Id & deckType.HasValue;
            })) {
                if (__instance is OxygenLeakGuy && s.storyVars.HasEverSeen("mezz_Ilya_Cobalt_0") && !s.storyVars.HasEverSeen("mezz_Ilya_Oxygenguy_Midcombat")) {
                    c.Queue(new ADelay() {
                        time = 0.0,
                        timer = 0.3,
                    });
                    c.Queue(new AMidCombatDialogue() {
                        script = "mezz_Ilya_Oxygenguy_Midcombat"
                    });
                    c.Queue(new ADelay() {
                        time = 0.0,
                        timer = 0.5,
                    });
                } else if (__instance is AsteroidDriller && s.storyVars.HasEverSeen("mezz_Ilya_Cobalt_0") && !s.storyVars.HasEverSeen("mezz_Ilya_Skunk_Midcombat")) {
                    c.Queue(new ADelay() {
                        time = 0.0,
                        timer = 0.3,
                    });
                    c.Queue(new AMidCombatDialogue() {
                        script = "mezz_Ilya_Skunk_Midcombat"
                    });
                    c.Queue(new ADelay() {
                        time = 0.0,
                        timer = 0.5,
                    });
                }
            }
            return true;
        }

        public static uint ColorToInt(System.Drawing.Color input) => 
            (uint)((Mutil.Clamp((int)(input.A * 255.0), 0, 255) << 24) | 
                (Mutil.Clamp((int)(input.R * 255.0), 0, 255) << 16) | 
                (Mutil.Clamp((int)(input.G * 255.0), 0, 255) << 8) | 
                Mutil.Clamp((int)(input.B * 255.0), 0, 255));

        public static bool LookupColorPrefix(ref uint? __result, string key) {
            if (key.Equals("TwosCompany.NolaDeck"))
                __result = ColorToInt(Manifest.NolaColor);
            else if (key.Equals("TwosCompany.IsabelleDeck"))
                __result = ColorToInt(Manifest.IsabelleColor);
            else if (key.Equals("TwosCompany.IlyaDeck"))
                __result = ColorToInt(Manifest.IlyaColor);
            else if (key.Equals("TwosCompany.JostDeck"))
                __result = ColorToInt(Manifest.JostColor);
            else if (key.Equals("TwosCompany.GaussDeck"))
                __result = ColorToInt(Manifest.GaussColor);
            else
                return true;

            return false;
        }
        public static void DialogueMusicPostfix(Dialogue __instance, ref MusicState? __result, G g) {
            if (__instance.ctx.script.Equals("mezz_Ilya_Memory_3"))
                __result = new MusicState() { scene = Song.SlowSilence };
            else if (__instance.ctx.script.Equals("mezz_Jost_Memory_3"))
                __result = new MusicState() { scene = Song.SlowSilence };
            else if (__instance.ctx.script.Equals("mezz_Ilya_Memory_3"))
                __result = new MusicState() { scene = Song.SlowSilence };
            else if (__instance.ctx.script.Equals("mezz_Gauss_Memory_2") &&
                __instance.bg is BGShipyard bg2)
                __result = bg2.GetMusicState();
            else if (__instance.bg is BGRunStartScripted bg3)
                __result = bg3.GetMusicState();
            return;
        }

        public static void CrystallizedFriendPostfix(ref List<Choice> __result, State s) {
            bool found = false;
            foreach (Choice choice in __result) {
                foreach(CardAction action in choice.actions) {
                    if (action is AAddCharacter charAdd && charAdd.deck.Key().Equals(ManifHelper.charStoryNames["jost"])) {
                        found = true;
                        choice.actions.Add(new AAddArtifact() {
                            artifact = new CrumpledWrit(),
                        });
                        break;
                    }
                    if (found)
                        break;
                }
            }
            return;
        }
        public static void ForeignCardOfferingPostfix(ref List<Choice> __result, State s) {
            bool found = false;
            foreach (Choice choice in __result) {
                foreach (CardAction action in choice.actions) {
                    if (action is ACardOffering cardAdd && ((int)cardAdd.limitDeck.GetValueOrDefault()) == ManifHelper.GetDeckId("jost")) {
                        found = true;
                        choice.actions.Add(new AAddArtifact() {
                            artifact = new CrumpledWrit(),
                        });
                        break;
                    }
                }
                if (found)
                    break;
            }
            return;
        }

        [HarmonyPriority(Priority.Last)]
        public static void RelockChars(ref HashSet<Deck> __result) {
            if (__result.Contains((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("nola")),
                        typeof(Deck)))) {
                __result.Remove((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("nola")),
                        typeof(Deck)));
            }

            if (__result.Contains((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("isa")),
                        typeof(Deck)))) {
                __result.Remove((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("isa")),
                        typeof(Deck)));
            }

            if (__result.Contains((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("ilya")),
                        typeof(Deck)))) {
                __result.Remove((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("ilya")),
                        typeof(Deck)));
            }

            if (__result.Contains((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("jost")),
                        typeof(Deck)))) {
                __result.Remove((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("jost")),
                        typeof(Deck)));
            }

            if (__result.Contains((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("gauss")),
                        typeof(Deck)))) {
                __result.Remove((Deck)Convert.ChangeType(Enum.ToObject(typeof(Deck), ManifHelper.GetDeckId("gauss")),
                        typeof(Deck)));
            }
        }


        public static bool OxygenLeakGuyCombatStartPrefix(AI __instance, State s, Combat c) {
            if (Enumerable.Any(s.characters, ch => {
                Deck? deckType = ch.deckType;
                return (int)deckType.GetValueOrDefault() == Manifest.IlyaDeck!.Id & deckType.HasValue;
            })) {
                if (s.storyVars.HasEverSeen("mezz_Ilya_Cobalt_0") && !s.storyVars.HasEverSeen("mezz_Ilya_Oxygenguy_Midcombat")) {
                    c.Queue(new ADelay() {
                        time = 0.0,
                        timer = 0.3,
                    });
                    c.Queue(new AMidCombatDialogue() {
                        script = "mezz_Ilya_Oxygenguy_Midcombat"
                    });
                    c.Queue(new ADelay() {
                        time = 0.0,
                        timer = 0.5,
                    });
                }
            }
            return true;
        }


    }
}
