using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class TwinMaleCable : Artifact {
        public override string Description() => "<c=keyword>+1</c> " + Manifest.ChainColH + "chain lightning</c> damage. " +
            "Whenever you split " + Manifest.ChainColH + "chain lightning</c>, " +
            "combine the split damage into the <c=redd>right end</c> instead of firing from the <c=attackFail>left end</c>.";

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