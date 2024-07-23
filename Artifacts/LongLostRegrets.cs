﻿using System.Diagnostics.Metrics;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class LongLostRegrets : Artifact {
        public override string Description() => "Gain 1 extra <c=energy>ENERGY</c> every turn. " +
            "<c=downside>The first time you receive hull damage each turn, gain two</c> <c=card>Fears</c>.";

        bool wasHit = false;

        public override void OnReceiveArtifact(State state) => ++state.ship.baseEnergy;
        public override void OnRemoveArtifact(State state) => --state.ship.baseEnergy;

        public override void OnTurnStart(State state, Combat combat) {
            if (wasHit) {
                wasHit = false;
            }
        }

        public override void OnPlayerLoseHull(State state, Combat combat, int amount) {
            if (!wasHit) {
                wasHit = true;
                this.Pulse();
                combat.Queue(new AAddCard() {
                    card = new Fear() {
                        temporaryOverride = false,
                        buoyantOverride = true,
                        buoyantOverrideIsPermanent = true,
                    },
                    amount = 2,
                    destination = CardDestination.Hand,
                });
            }
        }
        public override List<Tooltip>? GetExtraTooltips()
             => new List<Tooltip> { new TTGlossary("action.addCard", "<c=deck>hand</c>"),
                new TTCard() {
                    card = new Fear() {
                        temporaryOverride = false,
                        buoyantOverride = true,
                    },
                }
            };
    }
}