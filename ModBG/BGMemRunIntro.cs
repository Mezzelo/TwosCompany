using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using daisyowl.text;
using Microsoft.Extensions.Logging;
using TwosCompany.Helper;

namespace TwosCompany.ModBG {
    public class BGMemRunIntro : BG {

	    public static double DURATION = 10.0;
        public string voidLine = "THIS IS NOT THE END";
        public double timer;
        public double textTimer;
        public double pauseTimer = 0.0;
        private double pendingParticles;

        private int progress = 0;
        private int nlCount = 1;

        private ParticleSystem pfx = new ParticleSystem {
            blend = BlendMode.Screen,
            gradient = new List<Color>
                {
                Colors.black,
                Colors.textMain,
                Colors.black
            }
        };
        public override void OnAction(State s, string action) {
            voidLine = action switch {
                "nola" => "It'd be easier not to care, right?\n" +
                "But try as you might, you'll never be able to leave it all behind.\n" +
                "We are but mere beings with souls.",
                "isa" => "I know you're afraid. We all are.\n" +
                "Hold onto that.\n" +
                "With others relying on us, there is no worse fate than to no longer fear death.",
                "ilya" => "Breathe in. Breathe out.\n" +
                "That flame you feel, whatever it is, never let it go out.\n" +
                "When the stars grow distant, it will be all you have.",
                "jost" => "I know it's hard. Just look at me - I do.\n" +
                "But you have to keep going. One foot in front of the other.\n" +
                "No matter what happens, just keep moving forwards.",
                "gauss" => "We all deserve better.\n" +
                "Even the ones who did this to us - they are but victims of their own cruelty.\n" +
                "All I can promise you is something kinder.",
                _ => "NOT FOUND",
            };
        }

        public override bool IsWaiting(State s) {
            if (timer < DURATION) {
                return true;
            }
            return false;
        }

        public override void Render(G g, double t, Vec offset) {
            timer += g.dt;
            g.state.shake = 0.0;

            pendingParticles += g.dt * 1000.0 * Mutil.RemapClamped(0.1, 1.0, 0.0, 1.0, timer / DURATION);
            while (pendingParticles > 0.0) {
                Vec vec = Mutil.RandVel() * 200.0;
                vec += vec.normalized() * 70.0;
                pfx.Add(new Particle {
                    pos = G.screenSize / 2.0 + vec,
                    vel = vec * 0.5,
                    lifetime = 2.0,
                    size = Mutil.NextRand() * 3.0,
                    sizeMode = SizeMode.Constant
                });
                pendingParticles -= 1.0;
            }
            foreach (Particle particle in pfx.particles) {
                particle.pos = (particle.pos - G.screenSize / 2.0).rotate(0.0 - g.dt) + G.screenSize / 2.0;
            }
            Color? color = Colors.textMain;
            TAlign? align = TAlign.Center;
            if (pauseTimer == 0.0) {
                textTimer += g.dt;
                double num = Mutil.Remap(DURATION * 0.2, DURATION * 0.8, 0.0, voidLine.Count(), textTimer * 1.5);
                progress = (int) Math.Clamp(num, 0, voidLine.Count() - 1);
                if (voidLine.Substring(0, progress).Split('\n').Length > nlCount) {
                    do {
                        progress--;
                    } while (voidLine.Substring(0, progress).Split('\n').Length > nlCount);
                    progress -= (nlCount - 1);
                    nlCount++;
                    pauseTimer += g.dt;
                }
            }
            else if (pauseTimer > 0.0) {
                pauseTimer += g.dt;
                if (pauseTimer >= 1.0) {
                    pauseTimer = 0.0;
                }
            }
            Draw.Text(voidLine, 240.0, 128.0, null, color, null, progress, null, align);
            Draw.Rect(-1000.0, -1000.0, 2000.0, 2000.0, new Color(0.0, 0.0, 0.0, Mutil.Remap(DURATION * 0.1, DURATION * 0.5, 1.0, 0.0, timer)));
            Draw.Rect(-1000.0, -1000.0, 2000.0, 2000.0, new Color(1.0, 1.0, 1.0, Mutil.Remap(DURATION * 0.6, DURATION * 0.9, 0.0, 1.0, timer)));
            pfx.Render(g.dt * timer / DURATION * 5.0);
        }
    }
}
