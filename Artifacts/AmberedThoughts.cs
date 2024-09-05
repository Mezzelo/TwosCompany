using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class AmberedThoughts : Artifact, IAssignableArtifact {
        public bool proc = false;
        public int assignedUUID = -1;
        public TTCard? cardImpression;
        int IAssignableArtifact.assignedUUID { set => assignedUUID = value; }
        TTCard IAssignableArtifact.cardImpression { set => cardImpression = value; }
        public override string Description() => "Choose a card in your deck. The first time you play it each turn, gain 1 <c=status>DRONESHIFT</c>. " +
                "You also gain 1 <c=status>BULLET TIME</c> if you do not have any.";

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
                if (state.ship.Get((Status)Manifest.Statuses?["BulletTime"].Id!) <= 0) {
                    state.ship.Add((Status)Manifest.Statuses?["BulletTime"].Id!, 1 + state.ship.Get(Status.boost));
                    state.ship.Set(Status.boost, 0);
                }
                combat.QueueImmediate(new AStatus() {
                    status = Status.droneShift,
                    statusAmount = 1,
                    targetPlayer = true,
                    mode = AStatusMode.Add,
                    dialogueSelector = ".mezz_amberedThoughts",
                });
                proc = true;
            }
        }

        public override void OnTurnEnd(State state, Combat combat) {
            proc = false;
        }


        public override void OnCombatStart(State state, Combat c) {
            proc = false;
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
            list.Add(new TTGlossary("status.droneShift"));
            list.Add(new TTGlossary("status." + Manifest.Statuses?["BulletTime"].Id));
            if (assignedUUID != -1 && cardImpression != null)
                list.Add(cardImpression);
            return list;
        }
    }
}