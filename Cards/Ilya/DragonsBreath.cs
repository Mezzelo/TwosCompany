﻿using Nickel.Legacy;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Ilya {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class DragonsBreath : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                flippable = upgrade == Upgrade.B,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["DragonsBreathCardSpriteFlip"] : Manifest.Sprites["DragonsBreathCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }
        private bool InsufficientStatus (State s, Status status, int cost) {
            if (s.route is Combat) {
                if (!s.ship.statusEffects.ContainsKey(status))
                    return true;
                return s.ship.statusEffects[status] < cost;
            }
            return true;
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            if (upgrade == Upgrade.B)
                actions.Add(new AAttack() {
                    damage = GetDmg(s, 1),
                    moveEnemy = -1
                });
            else {
                if (Manifest.hasKokoro)
                    actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                    Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                        Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                        amount: 1
                    ), new AAttack() {
                        damage = GetDmg(s, 1),
                        moveEnemy = -1,
                        fast = true,
                    }).AsCardAction);
                else
                    actions.Add(new StatCostAttack() {
                        action = new AAttack() {
                            damage = GetDmg(s, 1),
                            moveEnemy = -1,
                            fast = true,
                        },
                        statusReq = Status.heat,
                        statusCost = 1,
                        cumulative = 0,
                        moveEnemy = -1,
                        first = true,
                        cardFlipped = this.flipped
                    });
            }

            if (Manifest.hasKokoro) {
                for (int i = 0; i < (upgrade == Upgrade.A ? 3 : 2); i++) {
                    actions.Add(Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                    Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                        Manifest.KokoroApi!.ActionCosts.MakeStatusResource(Status.heat),
                        amount: 1
                    ), new AAttack() {
                        damage = GetDmg(s, i > 0 ? 2 : 1),
                        moveEnemy = -1,
                        fast = true,
                    }).AsCardAction);
                }
                return actions;
            }
            actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    damage = GetDmg(s, 1),
                    moveEnemy = -1,
                    fast = true,
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = upgrade == Upgrade.B ? 0 : 1,
                first = upgrade == Upgrade.B,
                timer = -0.5,
                moveEnemy = -1,
                cardFlipped = this.flipped
            });
            actions.Add(new StatCostAttack() {
                action = new AAttack() {
                    damage = GetDmg(s, 2),
                    moveEnemy = -1,
                    fast = true,
                    dialogueSelector = ".mezz_dragonsBreath",
                },
                statusReq = Status.heat,
                statusCost = 1,
                cumulative = upgrade == Upgrade.B ? 1 : 2,
                timer = -0.5,
                moveEnemy = -1,
                cardFlipped = this.flipped
            });
            if (upgrade == Upgrade.A)
                actions.Add(new StatCostAttack() {
                    action = new AAttack() {
                        damage = GetDmg(s, 2),
                        moveEnemy = -2,
                        fast = true,
                    },
                    statusReq = Status.heat,
                    statusCost = 1,
                    cumulative = 3,
                    timer = -0.5,
                    moveEnemy = -2,
                    cardFlipped = this.flipped
                });
            return actions;
        }

        public override string Name() => "Dragon's Breath";
    }
}
