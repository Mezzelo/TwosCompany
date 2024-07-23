using System;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TwosCompany.Fx {

    public class FrozenHit : FX {
        public Vec pos;

        public static List<Spr> sprites = DB.GetSpriteListOrdered("effects_shieldHit_shieldHit_");

        public override void Render(G g, Vec v) {
            double num = 0.3;
            if (age < num) {
                double num2 = age / num;
                int num3 = 1;
                Spr? clamped = sprites[(int) (20.0 - num2 * 20.0)];
                double x = v.x + pos.x;
                double y = v.y + pos.y;
                Vec? originRel = new Vec(0.5, 0.5);
                BlendState add = BlendMode.Add;
                Color? color = new Color(1.0, 0.2, 0.2).gain(1.25);
                Draw.Sprite(clamped, x, y, flipX: false, flipY: false, 0.0, null, originRel, null, null, color, add);
            }
        }
    }
}
