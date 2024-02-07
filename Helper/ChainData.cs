using FSPRO;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace TwosCompany.Helper {
    public static class ChainData {

        public static Dictionary<int, int> hilights = new Dictionary<int, int>();
        public static int chainTimer = 0;
        public static int lastShipX = 0;

        public static byte[]? perlinColors;
        public static int texWidth = 128;
        public static int texHeight = 6;
        public static int texSize = texWidth * texHeight;

        public static byte[] LoadPerlin(G g) {
            Texture2D perlin = Texture2D.FromFile(g.mg.GraphicsDevice, 
                Path.Combine(Manifest.Instance.ModRootFolder!.FullName, "Sprites", "fx", "mezz_perlinRoughNormalized.png"));
            Microsoft.Xna.Framework.Color[] colorsXNA = new Microsoft.Xna.Framework.Color[perlin.Width * perlin.Height];
            byte[] colors1D = new byte[perlin.Width * perlin.Height];
            perlin.GetData(colorsXNA);
            for (int i = 0; i < perlin.Width * perlin.Height; i++) {
                colors1D[i] = colorsXNA[i].R;
            }
            return colors1D;
        }

        static double spread = 2.0;
        static Color outer = new Color(0.0, 0.15, 1.0);
        static Color outerTrail = new Color(0.0, 0.05, 1.0);
        static Color inner = new Color(0.3, 0.7, 1.0);
        static Color hitOuter = new Color(0.1, 0.6, 1.0);
        static Color hitInner = new Color(0.7, 0.9, 1.0);
        static Color trail = new Color(0.15, 0.15, 0.25);

        public static void ChainVFX(G g, Vec start, int dist, double ang, bool doTrail) {
            if (perlinColors == null)
                perlinColors = LoadPerlin(g);
            int noiseX = Mutil.NextRandInt() % texSize;
            double num2 = Mutil.NextRand();
            double angX = Math.Cos(Math.PI * ang / 180.0);
            double angXP = Math.Cos(Math.PI * (ang - 90.0) / 180.0);
            double angY = Math.Sin(Math.PI * ang / 180.0);
            double angYP = Math.Sin(Math.PI * (ang - 90.0) / 180.0);
            for (int i = 0; i < dist; i++) {
                for (int n = doTrail ? 0 : 1; n < 3; n++) {
                    if (n == 0 && i % 3 == 1)
                        continue;
                    Vec vel = Mutil.RandVel() * 5.0;
                    double randAng = Mutil.NextRand() * Math.PI * 2;
                    double spreadDist = Mutil.NextRand() * spread * Math.Max(0, 1 - n);
                    Vec pos = new Vec(
                            start.x
                                + (perlinColors[(noiseX + (int)(i / 2.25)) % texWidth + noiseX - (noiseX % texWidth)] / 255.0 * 12.0 - 6.0) * angXP
                                - i * angX,
                            start.y + (perlinColors[(noiseX + (int)(i / 2.25)) % texWidth + noiseX - (noiseX % texWidth)] / 255.0 * 12.0 - 6.0) * angYP
                                - i * angY
                        ) +
                        new Vec(Math.Cos(randAng) * spreadDist, Math.Sin(randAng) * spreadDist);
                    // old color formula: Color.Lerp(inner, outer, Math.Abs(Math.Cos(randAng - Math.PI * (ang - 90.0) / 180.0)) * spreadDist/spread),
                    PFX.combatAdd.Add(new Particle {
                        pos = pos,
                        size = n == 0 ? 1.0 + 1.0 * Mutil.NextRand() :
                            (n == 1 ? 5.0 : 1.2),
                        vel = vel * Math.Max(0, 1 - n),
                        color = n == 0 ? trail :
                            (n == 1 ? (doTrail ? outerTrail : outer) : (doTrail ? inner : hitInner)),
                        dragCoef = Mutil.NextRand() * Math.Max(0, 1 - n),
                        dragVel = new Vec(Math.Sin(num2 * 6.28 + pos.y * 0.3)) * 7.0 * Math.Max(0, 1 - n),
                        lifetime = n > 0 ? 0.1 :
                            0.3 + 1.4 * Mutil.NextRand(),
                        gravity = 0.0
                    });
                }
            }
        }
        public static void HullImpactLightning(G g, Vec hitPos, bool targetPlayer, bool isDirectional, bool fromDrone, bool doScatter) {
            int trailRand = Mutil.NextRandInt() % 2 + 3;
            if (doScatter)
                for (int n = 0; n < trailRand; n++)
                    ChainVFX(g, hitPos, Mutil.NextRandInt() % 20 + 10, Mutil.NextRand() * 80.0 + 230.0, false);
            
            for (int i = 0; i < 50; i++) {
                Vec vel = Mutil.RandVel() * 150.0;
                if (isDirectional) {
                    vel.y = (double)(targetPlayer ? (-3) : 3) * Math.Abs(vel.y);
                }

                if (fromDrone) {
                    vel.y *= 0.4;
                }

                PFX.combatAdd.Add(new Particle {
                    pos = hitPos,
                    size = 0.7 + 3.0 * Mutil.NextRand(),
                    vel = vel,
                    color = Color.Lerp(hitInner, hitOuter, vel.len()/215.0),
                    dragCoef = 12.0 + 4.0 * Mutil.NextRand(),
                    lifetime = 0.0 + 1.0 * Mutil.NextRand()
                });
            }
        }

        public static void BetweenDrones(G g, int fromX, int toX) {
            int start = fromX > toX ? toX : fromX;
            int end = fromX > toX ? fromX : toX;
            Vec startV = FxPositions.DroneCannon(start, false) + new Vec(5.0, -4.0 - Mutil.NextRand() * 4.0);
            ChainVFX(g, startV, (int) (FxPositions.DroneCannon(end, false).x - startV.x), -180.0, end - start > 1);
        }

        public static void Cannon(G g, bool targetPlayer, RaycastResult ray, DamageDone dmg) {
            Combat combat = (Combat) g.state.route;
            if (combat == null) {
                return;
            }
            // Vec start = FxPositions.DroneCannon(ray.worldX, targetPlayer);
            Rect rect = Rect.FromPoints(
                ray.fromDrone ? FxPositions.DroneCannon(ray.worldX, targetPlayer) + new Vec(0.0, -15.0) : FxPositions.Cannon(ray.worldX, !targetPlayer) + new Vec(0.0, 5.0), 
                (!ray.hitDrone && !ray.hitShip) ? FxPositions.Miss(ray.worldX, targetPlayer) : 
                    (ray.hitDrone ? FxPositions.Drone(ray.worldX) + new Vec(0.0, 6.0) : 
                        (dmg.hitHull ? FxPositions.Hull(ray.worldX, targetPlayer) : 
                            FxPositions.Shield(ray.worldX, targetPlayer)
                        )
                    )
            );
            ChainVFX(g, new Vec(rect.x + 3.0, rect.y + rect.h), (int) rect.h, 90.0, true);

            FMOD.GUID? clip = null;
            // Audio.Play(FSPRO.Event.Status_PowerUp);

            if (ray.hitShip) {
                Vec hitPos = new Vec(rect.x, targetPlayer ? rect.y2 : rect.y);
                HullImpactLightning(g, hitPos, targetPlayer, !ray.hitDrone, ray.fromDrone, dmg.hitHull);
            }

            if (dmg.hitShield && !dmg.hitHull) {
                combat.fx.Add(new ShieldHit {
                    pos = FxPositions.Shield(ray.worldX, targetPlayer)
                });
                ParticleBursts.ShieldImpact(g, FxPositions.Shield(ray.worldX, targetPlayer), targetPlayer);
            }

            if (dmg.poppedShield) {
                combat.fx.Add(new ShieldPop {
                    pos = FxPositions.Shield(ray.worldX, targetPlayer)
                });
            }

            if (dmg.poppedShield) {
                clip = FSPRO.Event.Hits_ShieldPop;
            }
            else if (dmg.hitShield) {
                clip = FSPRO.Event.Hits_ShieldHit;
            }

            if (!ray.hitDrone && !ray.hitShip) {
                clip = FSPRO.Event.Hits_Miss;
            }
            else if (dmg.hitHull) {
                clip = (!targetPlayer) ? FSPRO.Event.Hits_OutgoingHit : FSPRO.Event.Hits_HitHurt;
            }
            else if (ray.hitDrone) {
                clip = FSPRO.Event.Hits_HitDrone;
            }

            if (clip.HasValue) {
                Audio.Play(clip.Value);
            }
        }

    }
}



/*
Rect rect = Rect.FromPoints(ray.fromDrone ? 
        FxPositions.DroneCannon(ray.worldX, targetPlayer) : FxPositions.Cannon(ray.worldX, !targetPlayer), 
    (!ray.hitDrone && !ray.hitShip) ? 
        FxPositions.Miss(ray.worldX, targetPlayer) : 
            (ray.hitDrone ? FxPositions.Drone(ray.worldX) : 
                (dmg.hitHull ? FxPositions.Hull(ray.worldX, targetPlayer) : FxPositions.Shield(ray.worldX, targetPlayer))
            )
);
combat.fx.Add(new CannonBeam {
    rect = rect
});
ParticleBursts.CannonTrail(rect);
    if (ray.hitShip) {
        Vec hitPos = new Vec(rect.x, targetPlayer ? rect.y2 : rect.y);
        ParticleBursts.HullImpact(g, hitPos, targetPlayer, !ray.hitDrone, ray.fromDrone);
    } */