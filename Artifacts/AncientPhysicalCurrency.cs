using Microsoft.Xna.Framework.Media;
using TwosCompany.Actions;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AncientPhysicalCurrency : Artifact {
        public List<int> drawnCards = new List<int>();
        public bool queued = false;
        public override string Description() => "Cards that cost <c=keyword>2</c> or <c=keyword>4</c> <c=energy>ENERGY</c> are discounted by <c=keyword>1</c> when drawn.\r\n" +
            "Cards that cost <c=keyword>0</c> <c=energy>ENERGY</c> <c=downside>cost 1 more when drawn</c>.";
        public override void OnRemoveArtifact(State state) => drawnCards.Clear();
        public override void OnCombatStart(State state, Combat combat) => drawnCards.Clear();
        public override void OnPlayerRecieveCardMidCombat(State state, Combat combat, Card card) {
            drawnCards.Add(card.uuid);
            if (card.GetDataWithOverrides(state).cost == 2 || card.GetDataWithOverrides(state).cost == 4)
                card.discount --;
            else if (card.GetDataWithOverrides(state).cost == 0)
                card.discount++;
            else
                return;
            this.Pulse();
        }
        public override void OnTurnEnd(State state, Combat combat) => drawnCards.Clear();
        public override void OnDrawCard(State state, Combat combat, int count) {
            bool changedAny = false;
            foreach (Card card in combat.hand)
                if (!drawnCards.Contains(card.uuid)) {
                    drawnCards.Add(card.uuid);
                    if (card.GetDataWithOverrides(state).cost == 2 || card.GetDataWithOverrides(state).cost == 4)
                        card.discount--;
                    else if (card.GetDataWithOverrides(state).cost == 0)
                        card.discount++;
                    else
                        continue;
                    changedAny = true;
                }
            if (changedAny)
                this.Pulse();
        }

        public override void OnCombatEnd(State state) => drawnCards.Clear();
    }
}