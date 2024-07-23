using Microsoft.Xna.Framework;
using System;
using TwosCompany;
using TwosCompany.Actions;
using TwosCompany.Cards.Jost;
using TwosCompany.Fx;

namespace TwosCompany.Midrow {
    public class FrozenAttack : StuffBase {

        public bool unfreeze = false;
        public bool hadBubble = false;
        public List<AAttack> attacks = new List<AAttack>();
        public List<AAttack> attacksHostile = new List<AAttack>();

        static double particleInterval = 0.03;
        double particleStep = particleInterval;

        public override Spr? GetIcon() => 
            (Spr)(Manifest.Sprites["IconFrozenAttack"]?.Id ?? throw new Exception("missing frozen attack icon"));
        
        public override List<CardAction>? GetActions(State s, Combat c) {
            if ((s.ship.Get((Status) Manifest.Statuses["BulletTime"].Id!) > 0 
                || c.otherShip.Get((Status)Manifest.Statuses["BulletTime"].Id!) > 0)
                && !unfreeze)
                return null;
            return new List<CardAction>() {
                new ADrainFreeze() {
                    attack = this,
                    timer = 0.0,
                    noFx = true,
                }
            };
        }

        public override bool Invincible() {
            return true;
        }
        public void ImpactFx(Combat c, bool fromPlayer) {
            Vec hitPos = FxPositions.Drone(this.x) + new Vec(0, fromPlayer ? 2.0 : -2.0);

            for (int i = 0; i < 30; i++) {
                Vec vel = Mutil.RandVel() * 75.0;
                vel.y = (fromPlayer ? (-3.5) : 3.5) * Math.Abs(vel.y);

                PFX.combatAdd.Add(new Particle {
                    pos = hitPos,
                    size = 1.0 + 2.0 * Mutil.NextRand(),
                    vel = vel,
                    color = new Color(1.0, 0.4, 0.75),
                    dragCoef = 12.0 + 4.0 * Mutil.NextRand(),
                    lifetime = 0.0 + 0.5 * Mutil.NextRand()
                });
            }
            c.fx.Add(new FrozenHit {
                pos = hitPos
            });
        }

        public override List<CardAction>? GetActionsOnBonkedWhileInvincible(State s, Combat c, bool wasPlayer, StuffBase thing) {
            if (hadBubble)
                return null;
            if (c.currentCardAction is ASpawn spawn)
                spawn.timer = 0.0;
            return new List<CardAction>() {
                new AChainDestroyDrone() {
                    stuff = this,
                    playerDidIt = wasPlayer,
                    noFx = true,
                    fxOverride = false,
                    timer = 0.4,
                }
            };
        }

        public override List<CardAction>? GetActionsOnShotWhileInvincible(State s, Combat c, bool wasPlayer, int damage) {
            if (c.currentCardAction != null && c.currentCardAction.GetType() == typeof(AAttack) && c.currentCardAction is AAttack addAttack &&
                !addAttack.fromDroneX.HasValue && (addAttack.piercing || !hadBubble)) {
                ImpactFx(c, wasPlayer);
                AAttack newAttack = Mutil.DeepCopy(addAttack);
                newAttack.fromX = null;
                if (addAttack.targetPlayer)
                    attacksHostile.Add(newAttack);
                else
                    attacks.Add(newAttack);
            }
            return null;
        }

        public override string GetDialogueTag() => "conduit";

        public override double GetWiggleAmount() => 0.5;

        public override double GetWiggleRate() => 1;

        public override List<Tooltip> GetTooltips() {
            List<Tooltip> tooltipList = new List<Tooltip>();
            bool weaken = false;
            bool brittle = false;
            bool armorize = false;
            bool stun = false;
            int move = 0;
            // List<Status> statuses = new List<Status>();
            tooltipList.Add(new TTGlossary(
                Manifest.Glossary["FrozenAttack"].Head));
            List<Tooltip> tooltips = tooltipList;
            if (this.bubbleShield)
                tooltips.Add(new TTGlossary("midrow.bubbleShield"));
            foreach (AAttack attack in attacksHostile) {
                if (!stun && attack.stunEnemy) {
                    stun = true;
                    tooltips.Add(new TTGlossary("action.stun"));
                }
                if (!weaken && attack.weaken) {
                    weaken = true;
                    tooltips.Add(new TTGlossary("parttrait.weak"));
                }
                if (!brittle && attack.brittle) {
                    brittle = true;
                    tooltips.Add(new TTGlossary("parttrait.brittle"));
                }
                if (!armorize && attack.armorize) {
                    brittle = true;
                    tooltips.Add(new TTGlossary("parttrait.armor"));
                }
            }
            foreach (AAttack attack in attacks) {
                move += attack.moveEnemy;
                if (!stun && attack.stunEnemy) {
                    stun = true;
                    tooltips.Add(new TTGlossary("action.stun"));
                }
                if (!weaken && attack.weaken) {
                    weaken = true;
                    tooltips.Add(new TTGlossary("parttrait.weak"));
                }
                if (!brittle && attack.brittle) {
                    brittle = true;
                    tooltips.Add(new TTGlossary("parttrait.brittle"));
                }
                if (!armorize && attack.armorize) {
                    brittle = true;
                    tooltips.Add(new TTGlossary("parttrait.armor"));
                }
            }
            if (move != 0)
                tooltips.Add(new TTGlossary("action.move" + (move > 0 ? "Right" : "Left") + "Enemy", Math.Abs(move)));
            return tooltips;
        }

        public Tuple<int?, int?, bool, bool, bool> getDamage () {
            int? damageIncoming = attacksHostile.Count > 0 ? 0 : null;
            int? damageOutgoing = attacks.Count > 0 ? 0 : null;
            bool stun = false;
            int modIncoming = 0;
            bool statusIncoming = false;
            bool statusOutgoing = false;
            foreach (AAttack attack in attacksHostile) {
                damageIncoming += attack.damage;
                statusIncoming = statusIncoming || attack.status.HasValue;
            }
            foreach (AAttack attack in attacks) {
                damageOutgoing += attack.damage;
                stun = stun || attack.stunEnemy;
                statusOutgoing = statusOutgoing || attack.status.HasValue;
            }
            return Tuple.Create<int?, int?, bool, bool, bool>(damageIncoming, damageOutgoing, statusIncoming, statusOutgoing, stun);
        }

        public override void Render(G g, Vec v) {
            particleStep -= g.dt;
            Tuple<int?, int?, bool, bool, bool> damage = getDamage();
            if (damage.Item1 != null) {
                this.DrawWithHilight(g,
                    (Spr)(Manifest.Sprites["FrozenIncoming"]?.Id ?? throw new Exception("missing frozen attack sprite")),
                    v + this.GetOffset(g), false, false
                );
                if (particleStep <= 0.0) {
                    Vec vel = Mutil.RandVel() * 15.0;
                    vel.y = Math.Abs(vel.y) * -8;
                    PFX.combatAdd.Add(new Particle {
                        pos = FxPositions.Drone(this.x) + new Vec(0.5, -4.5) + this.GetOffset(g),
                        size = 1.0 + 1.0 * Mutil.NextRand(),
                        vel = vel,
                        color = new Color(1.0, 0.35, 0.65),
                        dragCoef = 4.0 + 4.0 * Mutil.NextRand(),
                        lifetime = 0.0 + 0.5 * Mutil.NextRand()
                    });
                }
                Draw.Text(((int)damage.Item1).ToString(), v.x + 10.0, v.y + 10.0,
                    color: damage.Item3 ? Colors.card : Colors.redd,
                    outline: Colors.black);
            }
            if (damage.Item2 != null) {
                this.DrawWithHilight(g,
                    (Spr)(Manifest.Sprites["FrozenOutgoing"]?.Id ?? throw new Exception("missing frozen attack sprite")),
                    v + this.GetOffset(g), false, false
                );
                if (particleStep <= 0.0) {
                    Vec vel = Mutil.RandVel() * 15.0;
                    vel.y = Math.Abs(vel.y) * 8;
                    PFX.combatAdd.Add(new Particle {
                        pos = FxPositions.Drone(this.x) + new Vec(0.5, 6.5) + this.GetOffset(g),
                        size = 1.0 + 1.0 * Mutil.NextRand(),
                        vel = vel,
                        color = new Color(1.0, 0.65, 0.35),
                        dragCoef = 4.0 + 4.0 * Mutil.NextRand(),
                        lifetime = 0.0 + 0.5 * Mutil.NextRand()
                    });
                }
                if (damage.Item5 || g.state.ship.Get((Status)Manifest.Statuses["FrozenStun"].Id!) > 0) {
                    Draw.Sprite(Enum.Parse<Spr>("icons_stun"), v.x + 14.0, v.y + 20.0);
                }
                Draw.Text(((int)damage.Item2).ToString(), v.x + 10.0, v.y + 20.0,
                    color: damage.Item4 ? Colors.card : Colors.droneOutline,
                    outline: Colors.black);
            }
            if (particleStep <= 0.0)
                particleStep += particleInterval;
        }
    }
}