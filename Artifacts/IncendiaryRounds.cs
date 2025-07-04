﻿using TwosCompany.Actions;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class IncendiaryRounds : Artifact {
        // public bool firstHit = true;
        public override string Description() => ManifArtifactHelper.artifactTexts["IncendiaryRounds"];
        public override void OnEnemyGetHit(State state, Combat combat, Part? part) {
            // if (firstHit) {
            //     firstHit = false;
                if (state.ship.Get(Status.heat) > 0) {
                    this.Pulse();
                combat.QueueImmediate(new AIncendOverheat() {
                    timer = 0.0,
                });
                combat.QueueImmediate(new AStatus() {
                    targetPlayer = false,
                    status = Status.heat,
                    statusAmount = Math.Max(state.ship.Get(Status.heat), 0),
                    timer = 0.0,
                });
            }
            // }
        }

        // public override void OnTurnEnd(State state, Combat combat) => firstHit = true;
        // public override void OnCombatEnd(State state) => firstHit = true;
        // public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.heat", 3) };
    }
}