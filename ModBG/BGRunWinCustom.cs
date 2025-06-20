using FSPRO;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TwosCompany;
using TwosCompany.Helper;

namespace TwosCompany.ModBG {
    public class BGRunWinCustom : BG {
        public static Color bgColor = new Color("39ffff");

        public double sparkTimer;

        public double? charPickTimer;

        public Deck? charPickDeck;

        public bool glitch = false;
        public bool silence = false;
        public bool riggs = false;

        public double timer;

        public override void OnAction(State s, string action) {
            if (action == "glitch_cue") {
                glitch = true;
                timer = 0.0;
            } else if (action == "toggle_silence") {
                silence = !silence;
            }
            else if (action == "toggle_riggs") {
                riggs = !riggs;
            }
            else if (action == "runwinwho_reset") {
                charPickDeck = null;
                s.storyVars.runWinChar = null;
                charPickTimer = null;
                int lev = s.storyVars.memoryUnlockLevel.GetValueOrDefault(ManifHelper.GetDeck("sorrel"));
                if (lev == 1)
                    s.storyVars.memoryUnlockLevel[ManifHelper.GetDeck("sorrel")] = 0;
                s.ChangeRoute(s.MakeRunWinRoute);
            }
            else if (ManifHelper.GetDeckId(action) != 0) {
                s.storyVars.runWinChar = ManifHelper.GetDeck(action);
                charPickDeck = ManifHelper.GetDeck(action);
                charPickTimer = new double?(0.0);
            }
        }
        public MusicState? GetMusicState() => new MusicState() {
                scene = riggs ? Song.Riggs : 
                        glitch || silence ? Song.SlowSilence : Song.Vault,
                combat = 0.0,
            };

        public override bool IsWaiting(State s) {
            if (timer < 6.0 || !glitch) {
                return true;
            }
            return false;
        }



        public override void Render(G g, double t, Vec offset) {
            if (!charPickTimer.HasValue)
                charPickTimer = new double?(0.0);
            if (charPickDeck != g.state.storyVars.runWinChar) {
                charPickDeck = g.state.storyVars.runWinChar;
                charPickTimer = (charPickDeck.HasValue ? new double?(0.0) : null);
            }
            if (charPickTimer.HasValue && timer < 2.5) {
                if (!glitch)
                    charPickTimer = Mutil.LerpDelta(charPickTimer.Value, 1.0, 1.0, g.dt);
                Draw.Fill(Colors.black.fadeAlpha(Math.Pow(charPickTimer.Value, 2.0) * 1.2));
                Vec offset2 = new Vec(10.0, 10.0) * g.state.time;
                BGComponents.NormalStars(g, g.state.time, offset2);
                BGComponents.RegularNebula(g, offset2, new Color(0.3, 0.0, 0.5).gain(0.2));
                Deck? runWinChar = g.state.storyVars.runWinChar;
                if (runWinChar.HasValue) {
                    Deck valueOrDefault = runWinChar.GetValueOrDefault();
                    if (BGRunWin.charFullBodySprites.TryGetValue(valueOrDefault, out var value)) {
                        double y = 152.0 + 150.0 * (1.0 - charPickTimer.Value);
                        Spr? id = value;
                        Vec? originRel = new Vec(0.5, 0.5);
                        if (glitch) {
                            SpriteUtil.GlitchSprite(id.Value, 240, (int) y, 240.0, new Vec(360.0, 270.0), 
                                (int)Math.Round(g.state.time * (timer > 1.5 ? 36.0 : 6.0)), 1.0, Colors.white);
                            Audio.Auto(Event.Scenes_ColdStart)?.setParameterByName("ColdStart", 1f);
                        }
                        else
                            Draw.Sprite(id, 240.0, y, flipX: false, flipY: false, 0.0, null, originRel);

                        /*
                        Draw.Sprite(StableSpr.effects_comp_board_void_2, 0.0, 0.0);
                        SpriteUtil.GlitchSprite(StableSpr.effects_comp_board_void, 0, 0, 240.0, new Vec(480.0, 270.0), (int)Math.Round(g.state.time * 3.0), 1.0, Colors.white);
                        */
                    }
                }
                DrawBigIsosphereGlitched(g);
                if (!glitch)
                    g.state.shake = Mutil.RemapClamped(0.0, 0.9, 2.0, 0.0, charPickTimer.Value);
                else if (timer < 2.5)
                    g.state.shake = Math.Clamp(timer, 0.5, 2.0);
            }
            if (glitch && timer < 2.5) {
                Color color = new Color(0.2, 0.0, 0.2).gain(1.0);
                sparkTimer -= g.dt;
                if (sparkTimer <= 0.0) {
                    sparkTimer = Mutil.NextRand() * 0.5 + 0.5;
                    PlayerScreenDamage.OneShot();
                    g.state.flash += color;
                }
                g.state.flash.r = Math.Max(g.state.flash.r, color.r);
                g.state.flash.g = Math.Max(g.state.flash.g, color.g);
                g.state.flash.b = Math.Max(g.state.flash.b, color.b);
            } else if (timer >= 2.5) {
                Draw.Fill(Colors.black, BlendState.Opaque);
                Audio.Auto(Event.Scenes_ColdStart)?.setParameterByName("ColdStart", 2f);
            }
            if (glitch) 
                timer += g.dt;

        }

        public static void DrawBigIsosphereGlitched(G g, Spr? spr = null) {
            SpriteUtil.GlitchSpriteBounded(
                spr ?? Enum.Parse<Spr>("bg_icosphere_close"), 0.0, 0.0, new Vec(480.0, 270.0), 
                seed: (int)Math.Round(g.state.time * 2.0), 
                color: Colors.white.gain(0.3), duty: 0.9, jitterOddsPerLine: 0.15, blend: BlendMode.Screen);
        }
    }
}