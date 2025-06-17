using System.Diagnostics.Metrics;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class FlawlessCore : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["FlawlessCore"];

        bool missed = false;
        public override Spr GetSprite() => (Spr)(Manifest.Sprites["IconFlawlessCore" + (missed ? "Off" : "")].Id
            ?? throw new Exception("missing artifact art: flawlesscore"));

        public override void OnReceiveArtifact(State state) => ++state.ship.baseEnergy;
        public override void OnRemoveArtifact(State state) => --state.ship.baseEnergy;

        public override void OnTurnStart(State state, Combat combat) {
            if (missed) {
                combat.Queue(new AEnergy() { changeAmount = -2 });
                missed = false;
            }
            this.Pulse();
        }

        public override void OnEnemyDodgePlayerAttack(State state, Combat combat) {
            missed = true;
        }

        public override void OnCombatEnd(State state) {
            missed = false;
        }
    }
}