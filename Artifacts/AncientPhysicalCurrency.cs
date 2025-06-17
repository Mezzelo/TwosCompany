using Microsoft.Xna.Framework.Media;
using Nickel;
using TwosCompany.Actions;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class AncientPhysicalCurrency : Artifact {
        public List<int> drawnCards = new List<int>();
        public int discountCount = 2;
        public int increaseCount = 1;
        public override string Description() => ManifArtifactHelper.artifactTexts["AncientPhysicalCurrency"];
        public override void OnRemoveArtifact(State state) => drawnCards.Clear();
        public override void OnCombatStart(State state, Combat combat) => drawnCards.Clear();
        public override void OnPlayerRecieveCardMidCombat(State state, Combat combat, Card card) {
            drawnCards.Add(card.uuid);
            if (card.GetDataWithOverrides(state).cost > 2 && discountCount > 0) {
                card.discount--;
                discountCount--;
            } else if (card.GetDataWithOverrides(state).cost < 2 && card.discount >= 0 && increaseCount > 0) {
                card.discount++;
                increaseCount--;
            }
            else
                return;
            this.Pulse();
        }
        public override void OnTurnEnd(State state, Combat combat) {
            List<int> held = new List<int>();
            foreach (Card card in combat.hand) {
                if (drawnCards.Contains(card.uuid)) {
                    held.Add(card.uuid);
                }
            }
            drawnCards.Clear();
            drawnCards.AddRange(held);
            discountCount = 2;
            increaseCount = 1;
        }
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card card, State state, Combat combat, int handPosition, int handCount) {
            if (!card.GetDataWithOverrides(state).infinite && drawnCards.Contains(card.uuid))
                drawnCards.Remove(card.uuid);
        }
        public override void OnDrawCard(State state, Combat combat, int count) {
            bool changedAny = false;
            foreach (Card card in combat.hand)
                if (!drawnCards.Contains(card.uuid)) {
                    drawnCards.Add(card.uuid);
                    if (card.GetDataWithOverrides(state).cost > 1 && discountCount > 0) {
                        card.discount--;
                        discountCount--;
                    } else if (card.GetDataWithOverrides(state).cost < 2 && card.discount >=0 && increaseCount > 0) {
                        card.discount++;
                        increaseCount--;
                    }
                    else
                        continue;
                    changedAny = true;
                }
            if (changedAny)
                this.Pulse();
        }

        public override void OnCombatEnd(State state) {
            drawnCards.Clear();
            discountCount = 2;
            increaseCount = 1;
        }
    }
}