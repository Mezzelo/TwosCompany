using TwosCompany;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool. })]
    public class Metronome : Artifact {
        public int counter = 0;
        public bool lastWasMove = false;

    }
}