using TwosCompany.Actions;

namespace TwosCompany {
    public class PatchLogic {

        public static bool Card_StatCostAction_Prefix(G g, State state, ref CardAction action, bool dontDraw) {
            int iconWidth = 6;
            Spr? id = null;
            Spr? idNotMet = null;

            Status statusReq;
            int statusCost = 0;
            int cumulative = 0;
            int statAmount = 0;

            if (dontDraw)
                return true;

            // ong this is so ugly lol ion got time to research this though
            if (action is StatCostAction) {
                StatCostAction cast = (StatCostAction) action;
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
            }
            else if (action is StatCostAttack) {
                StatCostAttack cast = (StatCostAttack) action;
                statusReq = cast.statusReq;
                statusCost = cast.statusCost;
                cumulative = cast.cumulative;
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
    }
}
