﻿using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class BlackfootSigil : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["BlackfootSigil"];

        public override void OnCombatStart(State state, Combat combat) {
            combat.QueueImmediate(new AAddCard() {
                card = new FreedomOfMovement(),
                destination = CardDestination.Hand,
                amount = 1,
                artifactPulse = this.Key(),
            });
        }

        public override List<Tooltip>? GetExtraTooltips() {
            List<Tooltip> extraTooltips = new List<Tooltip>();
            extraTooltips.Add(new TTCard() {
                card = new FreedomOfMovement()
            });
            return extraTooltips;
        }
    }

}