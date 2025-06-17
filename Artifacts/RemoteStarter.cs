using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany.Cards.Gauss;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class RemoteStarter : Artifact {

        public int count = 0;
        public bool turnSpent = false;
        public override string Description() => ManifArtifactHelper.artifactTexts["RemoteStarter"];
        public override Spr GetSprite() => (Spr)(Manifest.Sprites["IconRemoteStarter" + (turnSpent ? "Used" : "")].Id ?? throw new Exception("missing artifact icon: remote"));
        public override int? GetDisplayNumber(State s) => count;    

        public override void OnTurnStart(State state, Combat combat) {
            if (turnSpent && count > 3)
                Proc(combat);
            else
                turnSpent = false;
        }
        public override void OnPlayerDestroyDrone(State state, Combat combat) {
            count++;
            if (count > 3 && !turnSpent) {
                turnSpent = true;
                Proc(combat);
            }
        }

        private void Proc(Combat combat) {
            count -= 4;
            this.Pulse();
            combat.Queue(new AAddCard() {
                card = new ConduitCard() { exhaustOverride = true, temporaryOverride = true, discount = -1 },
                destination = CardDestination.Hand,
                dialogueSelector = ".mezz_remoteStarter",
            });
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTCard() {
                card = new ConduitCard() { exhaustOverride = true, temporaryOverride = true, discount = -1 },
            }
        };
    }
}