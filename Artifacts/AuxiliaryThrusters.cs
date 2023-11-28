using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AuxiliaryThrusters : Artifact {
        public int assignedUUID = -1;
        public TTCard? cardImpression;

        public override void OnReceiveArtifact(State state) {
            state.GetCurrentQueue().Add(new ACardSelect() {
                browseAction = new AAssignCardThrusters(),
                browseSource = CardBrowse.Source.Deck,
            });
        }

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            if (card.uuid == assignedUUID) {
                this.Pulse();
                combat.Queue(new AAddCard() {
                    card = new Recover() {
                        exhaustOverride = true,
                        temporaryOverride = true,
                        discount = -1,
                        forTooltip = false
                    },
                    destination = CardDestination.Hand
                });
            }
        }

        /*
        public override void OnCombatStart(State state, Combat c) {
            Card? assignedCard = state.FindCard(assignedUUID);
            if (assignedCard == null) {
                assignedUUID = -1;
                cardImpression = null;
            } else
                cardImpression = new TTCard() {
                    card = assignedCard.CopyWithNewId(),
                    showCardTraitTooltips = false
                };
            
        }*/

        public override void OnRemoveArtifact(State state) {
            assignedUUID = -1;
        }
        public override List<Tooltip>? GetExtraTooltips() {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTCard() {
                card = new Recover() { forTooltip = true },
            });
            if (assignedUUID != -1 && cardImpression != null)
                list.Add(cardImpression);
            return list;
        }
    }
}