using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Nola;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss })]
    public class CommandCenter : Artifact {
        public override string Description() => "Whenever you play <c=keyword>3</c> different colored cards in the same turn, gain 1 <c=energy>energy</c>.";
        public List<Deck> decks = new List<Deck>();
        public List<TTCard> cards = new List<TTCard>();
        public int count = 0;
        public override int? GetDisplayNumber(State s) => s.route is Combat && count < 3 ? count : null;
        public override void OnTurnEnd(State state, Combat combat) {
            decks.Clear();
            cards.Clear();
            count = 0;
        }
        public override void OnCombatEnd(State state) {
            decks.Clear();
            cards.Clear();
            count = 0;
        }
        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card playedCard, State state, Combat combat, int handPosition, int handCount) {
            if (count < 3) {
                if (!decks.Contains(playedCard.GetMeta().deck)) {
                    if (count < 2) {

                        decks.Add(playedCard.GetMeta().deck);
                        cards.Add(new TTCard() {
                            card = playedCard,
                            showCardTraitTooltips = false
                        });
                    } else {
                        this.Pulse();
                        combat.Queue(new AEnergy() {
                            changeAmount = 1,
                        });
                    }
                    count++;
                }
            }
        }
        public override List<Tooltip>? GetExtraTooltips() {
            if (cards.Count == 0 || count == 3)
                return null;
            List<Tooltip> list = new List<Tooltip>();
            foreach (TTCard card in cards)
                list.Add(card);
            return list;
        }
    }
}