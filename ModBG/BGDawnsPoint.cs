using FSPRO;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.ModBG {
    public class BGDawnsPoint : BG {
        public bool alarm;

        public bool beam;

        public bool beamKilled;

        public bool dark;

        public bool critical;

        public bool killSound;

        public bool ambience;

        public bool rumble;

        public double rumbleTimer;

        public bool cables;

        public bool isosphere;

        public bool riggs;

        public override void Render(G g, double t, Vec offset) {
            Color value = new Color(0.1, 0.2, 0.3).gain(1.0);
            Color color = new Color(0.0, 0.5, 1.0).gain(0.7 + Math.Sin(t * 2.0) * 0.1);
            Vec p = new Vec(395.0, 135.0);
            if (alarm) {
                value = new Color(1.0, 0.5, 0.5).gain(0.5 + ((Math.Sin(t * 10.0) > 0.0) ? 0.5 : 0.0));
                color = new Color(1.0, 0.5, 0.5).gain(1.0);
            }
            if (beam) {
                color = new Color(0.1, 0.4, 1.0);
            }
            if (critical) {
                color = new Color(1.0, 0.5, 1.0).gain(0.6);
            }
            Draw.Sprite(StableSpr.bg_cobaltChamber_bg, 0.0, 0.0);
            if (dark) {
                Draw.Rect(0.0, 0.0, 480.0, 270.0, new Color(0.0, 0.0, 0.0, 0.5));
            }
            Spr? id = StableSpr.bg_cobaltChamber_crystal_glow;
            double y = 135.0 + Math.Sin(t * 1.5) * 3.0;
            Vec? originRel = new Vec(0.5, 0.5);
            BlendState screen = BlendMode.Screen;
            Draw.Sprite(id, 395.0, y, flipX: false, flipY: false, 0.0, null, originRel, null, null, null, screen);
            if (beam) {
                Vec vec = new Vec(387.0, 63.0);
                Vec vec2 = new Vec(404.0, 207.0);
                double num = 1.0 * (Math.Sin(t * 100.0) * 0.5 + 0.5);
                double num2 = num + 2.0;
                Color color2 = new Color("75caff");
                Draw.Rect(vec.x + num, vec.y, vec2.x - vec.x - num * 2.0, vec2.y - vec.y, color2 * new Color(0.5, 0.5, 1.0), BlendMode.Screen);
                Draw.Rect(vec.x + num2, vec.y, vec2.x - vec.x - num2 * 2.0, vec2.y - vec.y, color2.gain(1.5));
                Glow.Draw(p, 500.0, color.normalize());
                Glow.Draw(p, 200.0, color.normalize());
            }
            double num3 = (beam ? (Math.Sin(t * 70.0) * 1.0) : 0.0);
            double num4 = (beam ? 0.0 : (Math.Sin(t * 1.5) * 3.0));
            Spr? id2 = StableSpr.bg_cobaltChamber_crystal;
            double x = 395.0 + num3;
            double y2 = 135.0 + num4;
            originRel = new Vec(0.5, 0.5);
            Draw.Sprite(id2, x, y2, flipX: false, flipY: false, 0.0, null, originRel);
            Spr? id3 = StableSpr.bg_cobaltChamber_room_inner;
            Color? color3 = value;
            Draw.Sprite(id3, 0.0, 0.0, flipX: false, flipY: false, 0.0, null, null, null, null, color3);
            Spr? id4 = StableSpr.bg_core_scene_dizzy_isaac;
            originRel = new Vec(0.0, 1.0);
            Draw.Sprite(id4, 50.0, 60.0, flipX: false, flipY: false, 0.0, null, originRel);
            Draw.Sprite(StableSpr.bg_cobaltChamber_room_outer, 0.0, 0.0);
            Spr? id5 = StableSpr.bg_cobaltChamber_glass;
            color3 = value;
            screen = BlendMode.Screen;
            Draw.Sprite(id5, 0.0, 0.0, flipX: false, flipY: false, 0.0, null, null, null, null, color3, screen);
            if (cables) {
                Spr? id6 = StableSpr.bg_cobaltChamber_cables;
                screen = BlendMode.Screen;
                Draw.Sprite(id6, 92.0, 62.0, flipX: false, flipY: false, 0.0, null, null, null, null, null, screen);
            }
            if (isosphere) {
                BGVault.DrawMediumIsosphere(g, new Vec(395.0, 135.0));
                BGVault.DrawDigitalCameraNoise(g);
                BGComponents.Lightning(g, new Vec(395.0, 135.0), new Color(0.1, 0.4, 1.0));
            }
            if (riggs) {
                Spr? id7 = StableSpr.bg_cobaltChamber_riggs;
                double y3 = 158.0 + Math.Sin(t * 2.0) * 2.0;
                color3 = new Color(1.0, 1.0, 1.0, 0.2);
                Draw.Sprite(id7, 428.0, y3, flipX: false, flipY: false, 0.0, null, null, null, null, color3);
            }
            if (critical) {
                Draw.Fill(new Color(1.0, 0.0, 0.5).gain(0.1 + ((Math.Sin(t * 10.0) > 0.0) ? 0.1 : 0.0)), BlendMode.Screen);
            }
            if (!dark || beam || alarm || critical) {
                Glow.Draw(p, 900.0, color);
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
            BGComponents.Letterbox();
            UpdateSounds();
        }

        public override void OnAction(State s, string action) {
            if (action == null) {
                return;
            }
            switch (action.Length) {
                case 8:
                    switch (action[0]) {
                        case 'a':
                            if (action == "alarm_on") {
                                alarm = true;
                            }
                            break;
                        case 'b':
                            if (action == "beam_off") {
                                beam = false;
                            }
                            break;
                        case 'd':
                            if (action == "dark_off") {
                                dark = false;
                            }
                            break;
                        case 'r':
                            if (action == "riggs_on") {
                                riggs = true;
                            }
                            break;
                    }
                    break;
                case 9:
                    switch (action[1]) {
                        case 'l':
                            if (action == "alarm_off") {
                                alarm = false;
                            }
                            break;
                        case 'u':
                            if (action == "rumble_on") {
                                rumble = true;
                            }
                            break;
                        case 'a':
                            if (action == "cables_on") {
                                cables = true;
                            }
                            break;
                        case 'i':
                            if (action == "riggs_off") {
                                riggs = false;
                            }
                            break;
                    }
                    break;
                case 7:
                    switch (action[0]) {
                        case 'b':
                            if (action == "beam_on") {
                                beam = true;
                            }
                            break;
                        case 'd':
                            if (action == "dark_on") {
                                dark = true;
                            }
                            break;
                    }
                    break;
                case 11:
                    switch (action[0]) {
                        case 'c':
                            if (action == "critical_on") {
                                critical = true;
                            }
                            break;
                        case 'a':
                            if (action == "ambience_on") {
                                ambience = true;
                            }
                            break;
                    }
                    break;
                case 12:
                    switch (action[0]) {
                        case 'c':
                            if (action == "critical_off") {
                                critical = false;
                            }
                            break;
                        case 'a':
                            if (action == "ambience_off") {
                                ambience = false;
                            }
                            break;
                        case 'i':
                            if (action == "isosphere_on") {
                                isosphere = true;
                            }
                            break;
                    }
                    break;
                case 14:
                    switch (action[0]) {
                        case 'b':
                            if (action == "beam_killed_on") {
                                beamKilled = true;
                            }
                            break;
                        case 'k':
                            if (action == "kill_sound_off") {
                                killSound = false;
                            }
                            break;
                    }
                    break;
                case 13:
                    switch (action[0]) {
                        case 'k':
                            if (action == "kill_sound_on") {
                                killSound = true;
                            }
                            break;
                        case 'i':
                            if (action == "isosphere_off") {
                                isosphere = false;
                            }
                            break;
                    }
                    break;
                case 10:
                    switch (action[0]) {
                        case 'r':
                            if (action == "rumble_off") {
                                rumble = false;
                            }
                            break;
                        case 'c':
                            if (action == "cables_off") {
                                cables = false;
                            }
                            break;
                    }
                    break;
                case 15:
                    if (action == "beam_killed_off") {
                        beamKilled = false;
                    }
                    break;
            }
        }

        public void UpdateSounds() {
            if (rumble) {
                Audio.Auto(Event.Scenes_CobaltCritical);
            }
            if (!killSound) {
                if (ambience) {
                    Audio.Auto(Event.Scenes_CoreAmbience);
                }
                if (alarm) {
                    Audio.Auto(Event.Scenes_CoreAlarmFromOutside);
                }
                if (beam || beamKilled) {
                    Audio.Auto(Event.Scenes_CoreLaser)?.setParameterByName("CoreMode", beamKilled ? 1 : 0);
                }
            }
        }
    }
}
