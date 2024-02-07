using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Media;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class MessengerBag : Artifact {
        public override string Description() => "Gain 3 <c=status>ONSLAUGHT</c> every turn. <c=downside>Draw 1 less card per turn.</c>";

        public override void OnReceiveArtifact(State state) => --state.ship.baseDraw;
        public override void OnRemoveArtifact(State state) => ++state.ship.baseDraw;
        public override void OnTurnStart(State state, Combat combat) {
            ExternalStatus onslaughtStatus = Manifest.Statuses?["Onslaught"] ?? throw new Exception("status missing: repeat");
            combat.Queue(new AStatus() {
                status = (Status) onslaughtStatus.Id!,
                statusAmount = 3,
                targetPlayer = true,
                artifactPulse = this.Key(),
            });
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { 
            new TTGlossary("status." + Manifest.Statuses?["Onslaught"].Id),
        };
    }
}