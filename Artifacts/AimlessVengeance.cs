using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Media;
using System;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class AimlessVengeance : Artifact {
        public override string Description() =>
            "Start each combat with 3 <c=status>OFF. STANCE</c>.";
        public override void OnReceiveArtifact(State state) {
            foreach (Artifact artifact in state.EnumerateAllArtifacts())
                if (artifact is CrumpledWrit writ) {
                    writ.hasVengeance = true;
                    break;
                }
        }
        public override void OnRemoveArtifact(State state) {
            foreach (Artifact artifact in state.EnumerateAllArtifacts())
                if (artifact is CrumpledWrit writ) {
                    writ.hasVengeance = false;
                    break;
                }
        }

        public override void OnCombatStart(State state, Combat combat) {
            combat.Queue(new AStatus() {
                status = (Status)Manifest.Statuses["OffensiveStance"].Id!,
                statusAmount = 3,
                targetPlayer = true,
                artifactPulse = this.Key(),
            });
        }

        public override List<Tooltip>? GetExtraTooltips()
             => new List<Tooltip> { new TTGlossary("status." + Manifest.Statuses?["OffensiveStance"].Id) };
    }
}