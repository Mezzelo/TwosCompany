using FSPRO;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.ModBG {
    public class BGCoreEmpty : BG {

        public override void Render(G g, double t, Vec offset) {
            Color value = new Color(0.1, 0.2, 0.3).gain(1.0);
            Color color = new Color(0.0, 0.5, 1.0).gain(0.7 + Math.Sin(t * 2.0) * 0.1);
            Vec p = new Vec(395.0, 135.0);
            Draw.Sprite(StableSpr.bg_cobaltChamber_bg, 0.0, 0.0);
            Spr? id = StableSpr.bg_cobaltChamber_crystal_glow;
            double y = 135.0 + Math.Sin(t * 1.5) * 3.0;
            Vec? originRel = new Vec(0.5, 0.5);
            BlendState screen = BlendMode.Screen;
            Draw.Sprite(id, 395.0, y, flipX: false, flipY: false, 0.0, null, originRel, null, null, null, screen);
            double num3 = (0.0);
            double num4 = ((Math.Sin(t * 1.5) * 3.0));
            Spr? id2 = StableSpr.bg_cobaltChamber_crystal;
            double x = 395.0 + num3;
            double y2 = 135.0 + num4;
            originRel = new Vec(0.5, 0.5);
            Draw.Sprite(id2, x, y2, flipX: false, flipY: false, 0.0, null, originRel);
            Spr? id3 = StableSpr.bg_cobaltChamber_room_inner;
            Color? color3 = value;
            Draw.Sprite(id3, 0.0, 0.0, flipX: false, flipY: false, 0.0, null, null, null, null, color3);
            Spr? id4 = (Spr) Manifest.Sprites["CoreSceneNolaPeri"].Id!;
            originRel = new Vec(0.0, 1.0);
            Draw.Sprite(id4, 50.0, 59.0, flipX: false, flipY: false, 0.0, null, originRel);
            Draw.Sprite(StableSpr.bg_cobaltChamber_room_outer, 0.0, 0.0);
            Spr? id5 = StableSpr.bg_cobaltChamber_glass;
            color3 = value;
            screen = BlendMode.Screen;
            Draw.Sprite(id5, 0.0, 0.0, flipX: false, flipY: false, 0.0, null, null, null, null, color3, screen);
            Glow.Draw(p, 900.0, color);
            BGComponents.Letterbox();
        }


    }
}
