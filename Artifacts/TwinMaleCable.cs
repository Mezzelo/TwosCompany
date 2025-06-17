using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class TwinMaleCable : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["TwinMaleCable"];

        public override int ModifyBaseDamage(
          int baseDamage,
          Card? card,
          State state,
          Combat? combat,
          bool fromPlayer) {
            if (!fromPlayer || card == null)
                return 0;
            Deck? deck = card?.GetMeta()?.deck;
            return deck.GetValueOrDefault().ToString().Equals(ManifHelper.GetDeckId("gauss").ToString()) ? 1 : 0;
        }
    }
}