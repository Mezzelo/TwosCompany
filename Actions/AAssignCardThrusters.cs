using System;
using TwosCompany.Artifacts;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Actions {
    public class AAssignCardThrusters : CardAction {
        public CallAndResponse? callCard;
        public bool recall = false;
        public int discount = 0;
        public override void Begin(G g, State s, Combat c) {
            if (this.selectedCard == null)
                return;
            foreach (Artifact enumerateAllArtifact in s.EnumerateAllArtifacts()) {
                if (enumerateAllArtifact is AuxiliaryThrusters cast) {
                    cast.assignedUUID = this.selectedCard.uuid;
                    cast.cardImpression = new TTCard() {
                        card = this.selectedCard.CopyWithNewId()
                    };
                    return;
                }
            }
        }
        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconCallAndResponseHint"].Id ?? throw new Exception("missing icon")), null, Colors.textMain);
        public override List<Tooltip> GetTooltips(State s) {
            List<Tooltip> list = new List<Tooltip>() { new TTGlossary(Manifest.Glossary["CallAndResponseHint"]?.Head ??
                throw new Exception("missing glossary entry: CallAndResponseHint"), discount == 0 ? "" : " <c=textMain>and lower its cost by</c> <c=energy>" + discount + "</c>") };
            if (recall && this.selectedCard != null) {
                Card card2 = selectedCard.CopyWithNewId();
                list.Add((Tooltip)new TTCard() {
                    card = card2,
                    showCardTraitTooltips = false
                });
            }
            return list;
        }
    }
}