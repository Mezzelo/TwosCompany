namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class FingerlessGloves : Artifact {
        public int counter = 0;
        public override string Description() => "Whenever you play 5 cards in a single turn, gain 1 <c=status>EVADE</c>.";
        public override int? GetDisplayNumber(State s) => s.route is Combat && counter < 5 ? counter : null;
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            counter++;
            if (counter == 5) {
                this.Pulse();
                combat.QueueImmediate(new AStatus() {
                    targetPlayer = true,
                    status = Status.evade,
                    statusAmount = 1,
                    dialogueSelector = ".mezz_fingerlessGloves",
                });
            }
        }

        public override void OnTurnEnd(State state, Combat combat) => counter = 0;
        public override void OnCombatEnd(State state) => counter = 0;
        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.evade", 1) };
    }
}