using System.Diagnostics.Metrics;
using TwosCompany.Actions;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class MilitiaArmband : Artifact {
        public int prevTotal = 0;
        public bool procced = false;
        public override string Description() => ManifArtifactHelper.artifactTexts["MilitiaArmband"];

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            combat.Queue(new AStanceTotalCheck() {
                artif = this,
                timer = 0.0,
            });
        }

        public override void OnReceiveArtifact(State state) {
            BurdenOfHindsight? burden = (BurdenOfHindsight?) state.EnumerateAllArtifacts().FirstOrDefault(e => e is BurdenOfHindsight);
            if (burden != null)
                burden.armband = this;
        }
        public override void OnRemoveArtifact(State state) {
            BurdenOfHindsight? burden = (BurdenOfHindsight?)state.EnumerateAllArtifacts().FirstOrDefault(e => e is BurdenOfHindsight);
            if (burden != null)
                burden.armband = null;
        }

        public override void OnTurnStart(State state, Combat combat) {
            procced = false;
            combat.Queue(new AStanceTotalCheck() {
                artif = this,
                timer = 0.0,
            });
        }
        public override void OnTurnEnd(State state, Combat combat) {
            combat.Queue(new AStanceTotalCheck() {
                artif = this,
                timer = 0.0,
            });
        }

        public override void OnCombatEnd(State state) {
            prevTotal = 0;
            procced = false;
        }

        public void Proc(Combat c) {
            if (procced)
                return;
            procced = true;
            c.Queue(new AStatus() {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = this.Key(),
                dialogueSelector = ".mezz_militiaArmband",
            });
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            new TTGlossary("status.overdrive", 1)
        };
    }
}