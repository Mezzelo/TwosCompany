namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AncientPhysicalCurrency : Artifact {
        List<int> drawnCards = new List<int>();

        public override void OnRemoveArtifact(State state) => drawnCards.Clear();
        public override void OnCombatStart(State state, Combat combat) => drawnCards.Clear();
        public override void OnPlayerRecieveCardMidCombat(State state, Combat combat, Card card) {
            if (card.GetDataWithOverrides(state).cost >= 4)
                card.discount -= 2;
            else if (card.GetDataWithOverrides(state).cost == 3)
                card.discount --;
            else if (card.GetDataWithOverrides(state).cost <= 0)
                card.discount++;
            else
                return;
            this.Pulse();
        }
        public override void OnTurnEnd(State state, Combat combat) => drawnCards.Clear();
        public override void OnDrawCard(State state, Combat combat, int count) {
            foreach (Card card in combat.hand)
                if (!drawnCards.Contains(card.uuid)) {
                    drawnCards.Add(card.uuid);
                    if (card.GetDataWithOverrides(state).cost >= 3)
                        card.discount--;
                    else if (card.GetDataWithOverrides(state).cost <= 0)
                        card.discount++;
                    else
                        continue;
                    this.Pulse();
                }
        }

        public override void OnCombatEnd(State state) => drawnCards.Clear();
    }
}