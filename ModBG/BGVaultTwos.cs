
using FSPRO;
using Microsoft.Extensions.Logging;
using System.Data;

namespace TwosCompany.ModBG {
    public class BGVaultTwos : BG {

        public bool alarm;
        public bool critical;
        public bool ambience;
        public bool rumble;
        public bool beam;
        public bool killSound;

        public double rumbleTimer;
        public override void Render(G g, double t, Vec offset) {
            Dialogue dialogue = (Dialogue) g.state.route;

            Vec lookAway = dialogue?.ctx.script switch {
                "mezz_Nola_Memory_1" => new Vec(150.0, -60.0),
                "mezz_Nola_Memory_2" => new Vec(150.0, -60.0),
                "mezz_Isa_Memory_2" => new Vec(120.0),
                "mezz_Ilya_Memory_3" => new Vec(150.0),
                _ => new Vec(0.0, 0.0),
            };
            bool showOverlook = dialogue?.ctx.script == "mezz_Nola_Memory_1" || dialogue?.ctx.script == "mezz_Nola_Memory_2";
            bool showShips = dialogue?.ctx.script == "mezz_Isa_Memory_2" || dialogue?.ctx.script == "mezz_Ilya_Memory_3";
            DrawVaultBg(g, t, lookAway, showOverlook, showOverlook, showShips, letterbox: true);
        }

        public void DrawVaultBg(G g, double t, Vec lookAway = default(Vec), bool showOverlook = false, bool extraDark = false, bool showShips = false, bool letterbox = false) {
            Vec offset = (showOverlook ? new Vec(0.0, 0.0) : new Vec(g.state.time * -30.0));
            offset += lookAway;
            Vec offset2 = new Vec(g.state.time * -10.0, g.state.time * -10.0) + lookAway;
            Vec vec = new Vec(240.0, 135.0) + lookAway;
            Color color = new Color(0.05, 0.7, 1.0);
            Vec vec2 = Vec.One;
            Color color2 = (extraDark ? new Color(0.0, 0.5, 1.0).gain(0.3) : new Color(0.0, 0.5, 1.0));
            Draw.Fill(Colors.black);
            BGComponents.NormalStars(g, g.state.time, offset);
            if (!extraDark) {
                BGComponents.RegularNebula(g, offset, new Color(0.0, 0.2, 0.3).gain(0.7));
            }
            Draw.Fill(color2.gain(0.1), BlendMode.Screen);
            Glow.Draw(vec, 500.0, color2.gain(1.0));
            Vec? vec3;
            if (showOverlook) {
                for (int i = 0; i < 6; i++) {
                    Spr? id = StableSpr.bg_shipyard_scaffolding;
                    double x = lookAway.x + -150.0 + (double)(i * 50);
                    Color? color3 = color2.gain(0.1);
                    vec3 = null;
                    Draw.Sprite(id, x, 0.0, flipX: false, flipY: false, 0.0, null, null, vec3, null, color3);
                }
                for (int j = 0; j < 6; j++) {
                    Spr? id2 = StableSpr.bg_shipyard_scaffolding;
                    double x2 = lookAway.x + 480.0 + 150.0 - (double)(j * 50);
                    Color? color3 = color2.gain(0.1);
                    vec3 = new Vec(1.0);
                    Draw.Sprite(id2, x2, 0.0, flipX: true, flipY: false, 0.0, null, vec3, null, null, color3);
                }
            }
            Draw.Fill(color2.gain(0.1), BlendMode.Screen);
            Spr? id4 = StableSpr.bg_cobalt_hero_back;
            double x3 = vec.x;
            double y2 = vec.y - 0.0;
            vec3 = new Vec(0.5, 0.5);
            Draw.Sprite(id4, x3, y2, flipX: false, flipY: false, 0.0, null, vec3);
            Spr? id5 = (StableSpr.effects_circles_circle_128);
            double x4 = vec.x;
            double y3 = vec.y;
            vec3 = vec2;
            Draw.Sprite(originRel: new Vec(0.5, 0.5), color: color.gain(1.0), id: id5, x: x4, y: y3, flipX: false, flipY: false, rotation: 0.0, originPx: null, scale: vec3);
            // if (!cobaltExploded)
            {
                Spr? id6 = StableSpr.bg_cobalt_hero_front;
                double x5 = vec.x;
                double y4 = vec.y + 0.0;
                Vec? originRel2 = new Vec(0.5, 0.5);
                Draw.Sprite(id6, x5, y4, flipX: false, flipY: false, 0.0, null, originRel2);
            }
            Draw.Fill(new Color(0.0, 0.5, 1.0).gain(0.1), BlendMode.Screen);
            if (showOverlook) {
                BGComponents.RegularGlowMono(g, new Vec(0.0, g.state.map.age * -100.0), new Color(0.0, 0.5, 1.0).gain(0.5));
                Spr? id7 = StableSpr.bg_shipyard_platform;
                Vec? originRel2 = new Vec(0.0, 1.0);
                Draw.Sprite(id7, 0.0, 200.0, flipX: true, flipY: false, 0.0, null, originRel2);
            }
            else if (showShips) {
                for (int j = 0; j < 6; j++) {

                    Spr? ids3 = (Spr)Manifest.Sprites["ShipIsabelle"].Id!;
                    double y = 155.0 + Math.Sin(g.state.time * 2.0);
                    color2 = color.gain(0.1);
                    Vec originRel3 = new Vec(0.0, 1.0);
                    Draw.Sprite(ids3, 70.0, y, flipX: false, flipY: false, 0.0, null, originRel3, null, null, color2);
                    Spr? ids4 = StableSpr.bg_hover_ship_b;
                    double ys2 = 165.0 + Math.Sin(g.state.time * 1.7);
                    originRel3 = new Vec(0.0, 1.0);
                    color2 = color.gain(0.1);
                    Draw.Sprite(ids4, 100.0, ys2, flipX: false, flipY: false, 0.0, null, originRel3, null, null, color2);
                }
            }
            if (critical) {
                Draw.Fill(new Color(1.0, 0.0, 0.5).gain(0.1 + ((Math.Sin(t * 10.0) > 0.0) ? 0.1 : 0.0)), BlendMode.Screen);
            }
            if (rumble) {
                rumbleTimer += g.dt;
            }
            if (rumbleTimer > 0.0) {
                g.state.shake = rumbleTimer;
                Draw.Fill(new Color(0.25, 0.5, 1.0).gain(rumbleTimer / 3.0), BlendMode.Screen);
                Draw.Fill(new Color(1.0, 1.0, 1.0).fadeAlpha(rumbleTimer / 4.0));
            }
            if (killSound) {
                g.state.shake = 0.0;
            }
            if (letterbox)
                BGComponents.Letterbox();
            UpdateSounds();
        }

        public static void DrawMediumIsosphere(G g, Vec sunPos) {
            if (Mutil.Rand((int)(g.state.time * 5.0)) < 0.8) {
                SpriteUtil.GlitchSprite(StableSpr.effects_icosphere, (int)sunPos.x - 85, (int)sunPos.y - 85, 240.0, new Vec(170.0, 170.0), (int)Math.Round(g.state.time * 10.0), 0.8, new Color(0.0, 0.2, 1.0).gain(1.0), BlendMode.Screen);
                return;
            }
            Spr? id = StableSpr.effects_icosphere;
            double x = sunPos.x;
            double y = sunPos.y;
            Vec? originRel = new Vec(0.5, 0.5);
            Color? color = new Color(0.0, 0.2, 1.0).gain(1.0);
            Draw.Sprite(blend: BlendMode.Screen, id: id, x: x, y: y, flipX: false, flipY: false, rotation: 0.0, originPx: null, originRel: originRel, scale: null, pixelRect: null, color: color);
        }

        public static void DrawDigitalCameraNoise(G g, double duty = 0.1, Color? color = null) {
            SpriteUtil.GlitchSprite(StableSpr.effects_glitch, 0, 0, 240.0, new Vec(480.0, 270.0), (int)Math.Round(g.state.time * 3.0), duty, color ?? new Color(0.0, 0.2, 1.0).gain(0.2), BlendMode.Screen);
        }

        public override void OnAction(State s, string action) {
            if (action.Equals("alarm_on"))
                alarm = true;
            else if (action.Equals("critical_on"))
                critical = true;
            else if (action.Equals("beam_on"))
                beam = true;
            else if (action.Equals("rumble_on"))
                rumble = true;
            else if (action.Equals("ambience_on"))
                ambience = true;
            else if (action.Equals("kill_sound_on"))
                killSound = true;
        }

        public void UpdateSounds() {
            if (rumble)
                Audio.Auto(Event.Scenes_CobaltCritical);

            if (!killSound) {
                if (ambience)
                    Audio.Auto(Event.Scenes_CoreAmbience);
                if (alarm)
                    Audio.Auto(Event.Scenes_CoreAlarmFromOutside);
            }
        }
    }
}
