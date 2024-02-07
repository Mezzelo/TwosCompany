using System;
using TwosCompany.Artifacts;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Actions {
    public class AAssignCardArtif : CardAction {
        public Artifact? assignArtifact;
        public override void Begin(G g, State s, Combat c) {
            if (this.selectedCard == null || assignArtifact == null)
                return;
            IAssignableArtifact? cast = (IAssignableArtifact?) s.EnumerateAllArtifacts().FirstOrDefault(e => e.Equals(assignArtifact));
            if (cast == null)
                return;
            cast.assignedUUID = this.selectedCard.uuid;
            cast.cardImpression = new TTCard() {
                card = this.selectedCard.CopyWithNewId(),
                showCardTraitTooltips = false
            };
            return;
        }
    }
}