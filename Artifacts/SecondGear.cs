using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class SecondGear : Artifact {
        public int counter = 0;
        public override string Description() => ManifArtifactHelper.artifactTexts["SecondGear"];
        public override int? GetDisplayNumber(State s) => counter > 5 || counter < 1 ? null : counter;

        public override void OnTurnStart(State state, Combat combat) {
            counter++;
            if (counter == 6) {
                Ship ship = state.ship;
                bool first = false;
                foreach (KeyValuePair<Status, int> thisStatus in ship.statusEffects) {
                    if (thisStatus.Value == 0)
                        continue;
                    if (
                        thisStatus.Key != Status.shield
                        && thisStatus.Key != Status.tempShield
                        // && thisStatus.Key != Status.maxShield
                        && thisStatus.Key != Status.shard
                    // && thisStatus.Key != Status.evade
                    // && thisStatus.Key != Status.maxShard
                        ) {

                        combat.Queue(new AStatus() {
                            targetPlayer = true,
                            status = thisStatus.Key,
                            statusAmount = 1,
                            mode = AStatusMode.Add,
                            dialogueSelector = first ? ".mezz_secondGear" : null,
                            artifactPulse = !first ? this.Key() : null,
                            timer = !first ? 0.4 : 0.0,
                        });
                        first = true;
                    }
                }
            }
        }

        public override void OnCombatEnd(State state) => counter = 0;
    }
}