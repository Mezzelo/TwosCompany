﻿using CobaltCoreModding.Definitions.ExternalItems;
using System;
using TwosCompany.Actions;
using TwosCompany.Artifacts;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" },
        dontOffer = true)]
    public class Heartbeat : Card, IJostCard {

        public bool breatheInRetain = false;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
                infinite = upgrade == Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade != Upgrade.B ? "Up1" : "") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.B ? 3 : 1,
                disabled = Stance.Get(s) % 2 != 1,
                timer = Stance.Get(s) == 3 ? 0.4 : 0.0,
            });

            actions.Add(new ADummyAction());

            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.B ? 3 : 1,
                disabled = Stance.Get(s) < 2,
                timer = Stance.Get(s) == 3 && upgrade == Upgrade.A ? 0.4 : 0.0,
            });
            if (upgrade != Upgrade.B) {
                
                    ExternalStatus defensiveStance = Manifest.Statuses["DefensiveStance"];

                    if (Manifest.hasKokoro) {
                        CardAction hbAction = Manifest.KokoroApi!.ActionCosts.MakeCostAction(
                            Manifest.KokoroApi!.ActionCosts.MakeResourceCost(
                                Manifest.KokoroApi!.ActionCosts.MakeStatusResource((Status) defensiveStance.Id!),
                                amount: 1
                            ), (upgrade == Upgrade.A ?
                                new AEnergy() {
                                    changeAmount = 1,
                                    disabled = Stance.Get(s) < 2,
                                } :
                                new ADrawCard() {
                                    count = 1,
                                    disabled = Stance.Get(s) < 2,
                                    timer = 0.0,
                                })
                        ).AsCardAction;
                        hbAction.disabled = Stance.Get(s) < 2;
                        actions.Add(hbAction);
                    } else
                        actions.Add(new StatCostAction() {
                            action = (upgrade == Upgrade.A ?
                                new AEnergy() {
                                    changeAmount = 1,
                                    disabled = Stance.Get(s) < 2,
                                } :
                                new ADrawCard() {
                                    count = 1,
                                    disabled = Stance.Get(s) < 2,
                                    timer = 0.0,
                                }),
                            statusReq = (Status) defensiveStance.Id!,
                            statusCost = 1,
                            first = true,
                            disabled = Stance.Get(s) < 2,
                        });

            }
            return actions;
        }

        public override void OnDraw(State s, Combat c) {
            MessengerBag? sigil = s.EnumerateAllArtifacts().OfType<MessengerBag>().FirstOrDefault();
            if (sigil != null && sigil.heartbeats < 1) {
                sigil.heartbeats++;
                s.ship.Add((Status)Manifest.Statuses?["Onslaught"].Id!, 1 + s.ship.Get(Status.boost));
                s.ship.Set(Status.boost, 0);
                sigil.Pulse();
            }
        }
        public override void AfterWasPlayed(State state, Combat c) {
            if (breatheInRetain) {
                breatheInRetain = false;
                retainOverride = false;
            }
        }
        public override void OnExitCombat(State s, Combat c) {
            if (breatheInRetain) {
                breatheInRetain = false;
                retainOverride = false;
            }
        }

        public override string Name() => "Heartbeat";
    }
}
