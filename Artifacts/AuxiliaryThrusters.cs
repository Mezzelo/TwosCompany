using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AuxiliaryThrusters : Artifact, IAssignableArtifact {
        public int assignedUUID = -1;
        public TTCard? cardImpression;
        int IAssignableArtifact.assignedUUID { set => assignedUUID = value; }
        TTCard IAssignableArtifact.cardImpression { set => cardImpression = value; }
        public override string Description() => "Choose a card in your deck.  Whenever you play that card, gain a <c=card>Recover</c>.";

        public override void OnReceiveArtifact(State state) {
            state.GetCurrentQueue().Insert(0, new ACardSelect() {
                browseAction = new AAssignCardArtif() {
                    assignArtifact = this,
                },
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
                        forTooltip = false,
                    },
                    destination = CardDestination.Hand,
                });
            }
        }

        
        public override void OnCombatStart(State state, Combat c) {
            Card? assignedCard = state.FindCard(assignedUUID);
            if (assignedCard != null && cardImpression != null) {
                if (assignedCard.buoyantOverrideIsPermanent)
                    cardImpression.card.buoyantOverride = assignedCard.buoyantOverride;
                if (assignedCard.exhaustOverrideIsPermanent)
                    cardImpression.card.exhaustOverride = assignedCard.exhaustOverride;
                if (assignedCard.recycleOverrideIsPermanent)
                    cardImpression.card.recycleOverride = assignedCard.recycleOverride;
                if (assignedCard.retainOverrideIsPermanent)
                    cardImpression.card.retainOverride = assignedCard.retainOverride;
                cardImpression.card.upgrade = assignedCard.upgrade;
            }

        }

        public override void OnRemoveArtifact(State state) {
            assignedUUID = -1;
        }
        public override List<Tooltip>? GetExtraTooltips() {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary("action.addCard", "<c=deck>hand</c>"));
            list.Add(new TTCard() {
                card = new Recover() { forTooltip = true },
            });
            if (assignedUUID != -1 && cardImpression != null)
                list.Add(cardImpression);
            return list;
        }
    }
}