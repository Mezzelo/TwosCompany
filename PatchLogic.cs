using System.Collections;
using System.Diagnostics.Metrics;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using TwosCompany.Actions;
using TwosCompany.Cards;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Nola;

namespace TwosCompany {
    public class PatchLogic {

        public static bool MoveBegin(AMove __instance, State s, Combat c, out int __state) {
            __state = __instance.targetPlayer ? s.ship.x : c.otherShip.x;
            bool flag = FeatureFlags.Debug && Input.shift;
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (!flag && ship == s.ship && (Enumerable.Any<TrashAnchor>(Enumerable.OfType<TrashAnchor>(c.hand)) ||
                ship.Get(Status.lockdown) > 0 || ship.Get(Status.engineStall) > 0))
                return true;
            ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
            if (strafeStatus.Id == null) return true;
            if (ship.Get((Status)strafeStatus.Id) > 0)
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

                Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
                ExternalStatus mobileStatus = Manifest.Statuses?["MobileDefense"] ?? throw new Exception("status missing: mobile defense");
                if (mobileStatus.Id != null)
                    if (ship.Get((Status)mobileStatus.Id) > 0)
                        c.QueueImmediate(new AStatus() {
                            status = Status.tempShield,
                            statusAmount = ship.Get((Status)mobileStatus.Id),
                            targetPlayer = __instance.targetPlayer,
                        });

            }
            return;
        }

        public struct healthStats {
            public healthStats(int hull, int shield, int tempShield) {
                this.hull = hull;
                this.shield = shield;
                this.tempShield = tempShield;
            }
            public int hull { get; }
            public int shield { get; }
            public int tempShield { get; }
        }
        public static bool AttackBegin(AAttack __instance, State s, Combat c, out healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            __state = new healthStats(ship.hull, ship.Get(Status.shield), ship.Get(Status.tempShield));

            return true;
        }
        public static void AttackEnd(AAttack __instance, State s, Combat c, healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (ship.hull < __state.hull || ship.Get(Status.shield) < __state.shield || ship.Get(Status.tempShield) < __state.tempShield) {
                ExternalStatus falseStatus = Manifest.Statuses?["FalseOpening"] ?? throw new Exception("status missing: falseopening");
                if (falseStatus.Id != null) {
                    if (ship.Get((Status)falseStatus.Id) > 0)
                        c.QueueImmediate(new AStatus() {
                            status = Status.overdrive,
                            statusAmount = ship.Get((Status)falseStatus.Id),
                            targetPlayer = __instance.targetPlayer,
                        });
                    ExternalStatus falseBStatus = Manifest.Statuses?["FalseOpeningB"] ?? throw new Exception("status missing: falseopeningb");
                    if (falseBStatus.Id != null) {
                        if (ship.Get((Status)falseBStatus.Id) > 0)
                            c.QueueImmediate(new AStatus() {
                                status = Status.powerdrive,
                                statusAmount = ship.Get((Status)falseBStatus.Id),
                                targetPlayer = __instance.targetPlayer,
                            });
                    }
                }
            }
            return;
        }

        public static bool MissileHitBegin(AMissileHit __instance, State s, Combat c, out healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            __state = new healthStats(ship.hull, ship.Get(Status.shield), ship.Get(Status.tempShield));

            return true;
        }
        public static void MissileHitEnd(AMissileHit __instance, State s, Combat c, healthStats __state) {
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (ship.hull < __state.hull || ship.Get(Status.shield) < __state.shield || ship.Get(Status.tempShield) < __state.tempShield) {
                ExternalStatus falseStatus = Manifest.Statuses?["FalseOpening"] ?? throw new Exception("status missing: falseopening");
                if (falseStatus.Id != null) {
                    if (ship.Get((Status)falseStatus.Id) > 0)
                        c.QueueImmediate(new AStatus() {
                            status = Status.overdrive,
                            statusAmount = ship.Get((Status)falseStatus.Id),
                            targetPlayer = __instance.targetPlayer,
                        });
                    ExternalStatus falseBStatus = Manifest.Statuses?["FalseOpeningB"] ?? throw new Exception("status missing: falseopeningb");
                    if (falseBStatus.Id != null) {
                        if (ship.Get((Status)falseBStatus.Id) > 0)
                            c.QueueImmediate(new AStatus() {
                                status = Status.powerdrive,
                                statusAmount = ship.Get((Status)falseBStatus.Id),
                                targetPlayer = __instance.targetPlayer,
                            });
                    }
                }
            }
            return;
        }

        public static bool TurnBegin(Ship __instance, State s, Combat c) {
            ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
            if (strafeStatus.Id != null) {
                if (__instance.Get((Status)strafeStatus.Id) > 0 && __instance.Get(Status.timeStop) <= 0)
                    __instance.Set((Status)strafeStatus.Id, 0);
            }


            ExternalStatus falseStatus = Manifest.Statuses?["FalseOpening"] ?? throw new Exception("status missing: falseopening");
            if (falseStatus.Id != null) {
                if (__instance.Get((Status)falseStatus.Id) > 0 && __instance.Get(Status.timeStop) <= 0)
                    __instance.Set((Status)falseStatus.Id, 0);
            }
            ExternalStatus falseBStatus = Manifest.Statuses?["FalseOpeningB"] ?? throw new Exception("status missing: falseopeningb");
            if (falseBStatus.Id != null) {
                if (__instance.Get((Status)falseBStatus.Id) > 0 && __instance.Get(Status.timeStop) <= 0)
                    __instance.Set((Status)falseBStatus.Id, 0);
            }
            /*
            ExternalStatus outmaneuverStatus = Manifest.Statuses?["Outmaneuver"] ?? throw new Exception("status missing: mobile defense");
            if (outmaneuverStatus.Id == null) return true;
            if (__instance.Get((Status)outmaneuverStatus.Id) > 0 && __instance.Get(Status.timeStop) <= 0)
                __instance.Set((Status)outmaneuverStatus.Id, 0);
            */

            ExternalStatus enflamedStatus = Manifest.Statuses?["Enflamed"] ?? throw new Exception("status missing: enflamed");
            if (enflamedStatus.Id == null) return true;
            if (__instance.Get((Status)enflamedStatus.Id) > 0) {
                c.QueueImmediate(new AStatus() {
                    status = Status.heat,
                    statusAmount = __instance.Get((Status)enflamedStatus.Id),
                    targetPlayer = __instance.isPlayerShip
                });
            }

            return true;
        }

        public static bool TurnEnd(Ship __instance, State s, Combat c) {
            ExternalStatus dodgeStatus = Manifest.Statuses?["UncannyEvasion"] ?? throw new Exception("status missing: uncanny evasion");
            if (dodgeStatus.Id == null) return true;
            if (__instance.Get((Status)dodgeStatus.Id) > 0 && __instance.Get(Status.shield) <= 0 && __instance.Get(Status.tempShield) <= 0)
                c.QueueImmediate(new AStatus() {
                    status = Status.autododgeRight,
                    statusAmount = __instance.Get((Status) dodgeStatus.Id),
                    targetPlayer = __instance.isPlayerShip
                });

            ExternalStatus mobileStatus = Manifest.Statuses?["MobileDefense"] ?? throw new Exception("status missing: mobile defense");
            if (mobileStatus.Id == null) return true;
            if (__instance.Get((Status)mobileStatus.Id) > 0 && __instance.Get(Status.timeStop) <= 0)
                __instance.Set((Status)mobileStatus.Id, __instance.Get((Status)mobileStatus.Id) - 1);

            ExternalStatus onslaughtStatus = Manifest.Statuses?["Onslaught"] ?? throw new Exception("status missing: repeat");
            if (onslaughtStatus.Id == null) return true;
            if (s.ship.Get((Status)onslaughtStatus.Id) > 0 && __instance.Get(Status.timeStop) <= 0)
                s.ship.Set((Status)onslaughtStatus.Id, 0);

            return true;
        }
        public static bool PlayCardPrefix(Combat __instance, State s, Card card, bool playNoMatterWhatForFree, bool exhaustNoMatterWhat) {
            /*
            ExternalStatus repeatStatus = Manifest.Statuses?["Repeat"] ?? throw new Exception("status missing: repeat");
            if (repeatStatus.Id == null) return true;

            if (s.ship.Get((Status)repeatStatus.Id) > 0) {
                s.ship.statusEffects[(Status)repeatStatus.Id]--;
                Card newCard = card.CopyWithNewId();
                newCard.exhaustOverride = true;
                newCard.temporaryOverride = true;

                __instance.SendCardToHand(s, newCard);
                __instance.TryPlayCard(s, newCard, true, exhaustNoMatterWhat);
            }*/
            return true;
        }

        public static void PlayCardPostfix(Combat __instance, ref bool __result, State s, Card card, bool playNoMatterWhatForFree, bool exhaustNoMatterWhat) {
            if (!__result)
                return;
            ExternalStatus onslaughtStatus = Manifest.Statuses?["Onslaught"] ?? throw new Exception("status missing: repeat");
            if (onslaughtStatus.Id == null) return;
            if (s.ship.Get((Status)onslaughtStatus.Id) > 0) {
                List<Card> cardList = s.deck;
                if (cardList.Count == 0) {
                    foreach (Card thisCard in __instance.discard)
                        s.SendCardToDeck(thisCard, true, true);
                    __instance.discard.Clear();
                    s.ShuffleDeck(true);
                }
                int count = 0;
                for (int drawIdx = cardList.Count - 1; drawIdx >= 0 && count < s.ship.statusEffects[(Status)onslaughtStatus.Id]; --drawIdx) {
                    Card selectCard = cardList[drawIdx];
                    if (selectCard.GetMeta().deck == card.GetMeta().deck) {
                        if (card.uuid != selectCard.uuid) {
                            if (__instance.hand.Count >= 10) {
                                __instance.PulseFullHandWarning();
                                break;
                            }
                            __instance.DrawCardIdx(s, drawIdx, CardDestination.Deck);
                            Audio.Play(FSPRO.Event.CardHandling);
                            count++;
                            continue;
                        }
                    }
                    /*
                    if (drawIdx == 0) {
                        (__instance).QueueImmediate(new AStatus() {
                            status = (Status)onslaughtStatus.Id,
                            statusAmount = 0,
                            mode = AStatusMode.Set,
                            targetPlayer = true,
                        });
                    } */
                }
            }
        }

        public static void CardDataPostfix(Card __instance, ref CardData __result, State state) {
            if (state.route is not Combat)
                return;
            if (!__result.flippable && state.ship.Get(Status.tableFlip) > 0) {
                if (__instance is Wildfire ||
                __instance is PointDefense ||
                __instance is AllHands) {
                    __result.flippable = true;
                }
            }
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

            if (action is StatCost cast) {
                statusReq = cast.statusReq;
                statusCost = cast.statusCost;
                cumulative =  cast.cumulative;
                if (cast.statusReq == Status.evade) {
                    id = (Spr)(Manifest.Sprites["IconEvadeCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconEvadeCostOff"].Id ?? throw new Exception("missing icon"));
                }
                else if (cast.statusReq == Status.shield) {
                    id = (Spr)(Manifest.Sprites["IconShieldCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconShieldCostOff"].Id ?? throw new Exception("missing icon"));
                }
                else if (cast.statusReq == Status.heat) {
                    id = (Spr)(Manifest.Sprites["IconHeatCost"].Id ?? throw new Exception("missing icon"));
                    idNotMet = (Spr)(Manifest.Sprites["IconHeatCostOff"].Id ?? throw new Exception("missing icon"));
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

        public static bool DisguisedCardName(Card __instance, ref string __result) {
            if (__instance is DisguisedCard) {
                if (((DisguisedCard)__instance).disguised) {
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
    }
}
