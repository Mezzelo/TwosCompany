using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class IonEngines : Artifact, IChainLightningArtifact {
        public int counter = 0;

        public override string Description() => ManifArtifactHelper.artifactTexts["IonEngines"];
        public override int? GetDisplayNumber(State s) => counter;

        public override void OnReceiveArtifact(State state) {
            counter = 0;
        }

        public override void OnRemoveArtifact(State state) {
            counter = 0;
        }

        public void OnChainLightning(State s, int distance) {
            counter += distance;
            while (counter > 5) {
                counter -= 5;
                this.Pulse();
               ((Combat) s.route).QueueImmediate(new AStatus() {
                   targetPlayer = true,
                   status = Status.evade,
                   statusAmount = 1
               });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            // new TTGlossary(Manifest.Glossary["ChainLightning"]!.Head, 1),
            new TTGlossary("status.evade", 1)
        };
    }
}