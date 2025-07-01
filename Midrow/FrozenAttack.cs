using Microsoft.Extensions.Logging;
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
                if (attack.cardOnHit != null) {
                    tooltips.Add(new TTCard() {
                        card = attack.cardOnHit,
                        showCardTraitTooltips = false
                    });
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
                if (attack.cardOnHit != null) {
                    tooltips.Add(new TTCard() {
                        card = attack.cardOnHit,
                        showCardTraitTooltips = false
                    });
                }
            }
            if (move != 0)
                tooltips.Add(new TTGlossary("action.move" + (move > 0 ? "Right" : "Left") + "Enemy", Math.Abs(move)));
            return tooltips;
        }

        public Tuple<int?, int?, byte, byte> getDamage () {
            int? damageIncoming = attacksHostile.Count > 0 ? 0 : null;
            int? damageOutgoing = attacks.Count > 0 ? 0 : null;
            byte modIncoming = 0;
            byte modOutgoing = 0;
            foreach (AAttack attack in attacksHostile) {
                damageIncoming += attack.damage;
                if (attack.status.HasValue)
                    modIncoming = (byte)(modIncoming | 1);
                if (attack.stunEnemy)
                    modIncoming = (byte)(modIncoming | 2);
                if (attack.weaken)
                    modIncoming = (byte)(modIncoming | 4);
                if (attack.brittle)
                    modIncoming = (byte)(modIncoming | 8);
                if (attack.armorize)
                    modIncoming = (byte)(modIncoming | 16);
                if (attack.cardOnHit != null)
                    modIncoming = (byte)(modIncoming | 32);

            }
            foreach (AAttack attack in attacks) {
                damageOutgoing += attack.damage;
                if (attack.status.HasValue)
                    modOutgoing = (byte)(modOutgoing | 1);
                if (attack.stunEnemy)
                    modOutgoing = (byte)(modOutgoing | 2);
                if (attack.weaken)
                    modOutgoing = (byte)(modOutgoing | 4);
                if (attack.brittle)
                    modOutgoing = (byte)(modOutgoing | 8);
                if (attack.armorize)
                    modOutgoing = (byte)(modOutgoing | 16);
                if (attack.cardOnHit != null)
                    modOutgoing = (byte)(modOutgoing | 32);
            }
            return Tuple.Create<int?, int?, byte, byte>(
                damageIncoming, damageOutgoing, modIncoming, modOutgoing);
        }

        public override void Render(G g, Vec v) {
            if (!targetPlayer) {
                foreach (AAttack attack in attacksHostile)
                    attack.targetPlayer = !attack.targetPlayer;
                foreach (AAttack attack in attacks)
                    attack.targetPlayer = !attack.targetPlayer;
                List<AAttack> transfer = [.. attacksHostile];
                attacksHostile = attacks;
                attacks = transfer;
                targetPlayer = true;
            }
            particleStep -= g.dt;
            Tuple<int?, int?, byte, byte> damage = getDamage();
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
                double iconPos = 10.0;
                if ((damage.Item3 & 2) > 0) {
                    Draw.Sprite(StableSpr.icons_stun, v.x + 14.0, v.y + iconPos, color: Colors.disabledText);
                    iconPos -= 10.0;
                }
                if ((damage.Item3 & 4) > 0) {
                    Draw.Sprite(StableSpr.icons_weak, v.x + 14.0, v.y + iconPos);
                    iconPos -= 10.0;
                }
                if ((damage.Item3 & 8) > 0) {
                    Draw.Sprite(StableSpr.icons_brittle, v.x + 14.0, v.y + iconPos);
                    iconPos -= 10.0;
                }
                if ((damage.Item3 & 16) > 0) {
                    Draw.Sprite(StableSpr.icons_armor, v.x + 14.0, v.y + iconPos);
                    iconPos -= 10.0;
                }
                if ((damage.Item3 & 32) > 0) {
                    Draw.Sprite(StableSpr.hints_hint_card, v.x + 11.0, v.y + iconPos - 3.0);
                }
                Draw.Text(((int)damage.Item1).ToString(), v.x + 10.0, v.y + 10.0,
                    color: (damage.Item3 & 1) > 0 ? Colors.card : Colors.redd,
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
                double iconPos = 20.0;
                if ((damage.Item4 & 2) > 0 || g.state.ship.Get((Status)Manifest.Statuses["FrozenStun"].Id!) > 0) {
                    Draw.Sprite(StableSpr.icons_stun, v.x + 14.0, v.y + iconPos);
                    iconPos += 10.0;
                }
                if ((damage.Item4 & 4) > 0) {
                    Draw.Sprite(StableSpr.icons_weak, v.x + 14.0, v.y + iconPos);
                    iconPos += 10.0;
                }
                if ((damage.Item4 & 8) > 0) {
                    Draw.Sprite(StableSpr.icons_brittle, v.x + 14.0, v.y + iconPos);
                    iconPos += 10.0;
                }
                if ((damage.Item4 & 16) > 0) {
                    Draw.Sprite(StableSpr.icons_armor, v.x + 14.0, v.y + iconPos);
                    iconPos += 10.0;
                }
                if ((damage.Item4 & 32) > 0) {
                    Draw.Sprite(StableSpr.hints_hint_card, v.x + 11.0, v.y + iconPos - 3.0, color: Colors.disabledText);
                }
                Draw.Text(((int) damage.Item2).ToString(), v.x + 10.0, v.y + 20.0,
                    color: (damage.Item4 & 1) > 0 ? Colors.card : Colors.droneOutline,
                    outline: Colors.black);
            }
            if (particleStep <= 0.0)
                particleStep += particleInterval;
        }
    }
}