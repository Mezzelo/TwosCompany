using System.Collections;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using TwosCompany.Actions;
using TwosCompany.Cards;

namespace TwosCompany {
    public class PatchLogic {

        public static bool MoveBegin(AMove __instance, G g, State s, Combat c) {
            bool flag = FeatureFlags.Debug && Input.shift;
            Ship ship = __instance.targetPlayer ? s.ship : c.otherShip;
            if (!flag && ship == s.ship && (Enumerable.Any<TrashAnchor>(Enumerable.OfType<TrashAnchor>(c.hand)) ||
                ship.Get(Status.lockdown) > 0 || ship.Get(Status.engineStall) > 0))
                return true;
            ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
            if (strafeStatus.Id == null) return true;
            if (ship.Get((Status) strafeStatus.Id) > 0)
                c.QueueImmediate(new AAttack() {
                    damage = Card.GetActualDamage(s, ship.Get((Status) strafeStatus.Id)),
                    targetPlayer = !__instance.targetPlayer,
                    fast = true,
                    storyFromStrafe = true
                });

            return true;
        }
        public static void MoveEnd(AMove __instance, G g, State s, Combat c) {
            return;
        }

        public static bool TurnBegin(Ship __instance, State s, Combat c) {
            ExternalStatus strafeStatus = Manifest.Statuses?["TempStrafe"] ?? throw new Exception("status missing: temp strafe");
            if (strafeStatus.Id == null) return true;
            if (__instance.Get(Status.timeStop) <= 0) {
                if (__instance.Get((Status)strafeStatus.Id) > 0)
                    __instance.Set((Status)strafeStatus.Id, 0);
            }

            return true;
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

            if (action is StatCost) {
                StatCost cast = (StatCost) action;
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
                    metCost = state.ship.statusEffects[statusReq] >= i + cumulative + 1;
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
