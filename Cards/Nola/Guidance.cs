using System;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Guidance : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["Guidance"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["Guidance"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["Guidance"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = 2,
                description = cardText,
                buoyant = upgrade == Upgrade.A,
                exhaust = true
            };
        }

        public List<Card> upgradedCards = new List<Card>();

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            
            actions.Add(new ADummyAction() {
                dialogueSelector = ".mezz_guidance"
            });

            return actions;
        }
        public override void AfterWasPlayed(State state, Combat c) {
            foreach (Card selectedCard in c.hand) {
                if (selectedCard.GetMeta().upgradesTo.Contains(upgrade == Upgrade.B ? Upgrade.B : Upgrade.A) &&
                    selectedCard.upgrade == Upgrade.None) {
                    upgradedCards.Add(selectedCard);
                    selectedCard.upgrade = upgrade == Upgrade.B ? Upgrade.B : Upgrade.A;
                }
            }
            Audio.Play(FSPRO.Event.Status_PowerUp);
        }
        public override void OnExitCombat(State s, Combat c) {
            foreach (Card searchCard in upgradedCards) {
                foreach (Card selectedCard in s.deck) {
                    if (searchCard.uuid.Equals(selectedCard.uuid) && selectedCard.upgrade == (upgrade == Upgrade.B ? Upgrade.B : Upgrade.A)) {
                        selectedCard.upgrade = Upgrade.None;
                    }
                }
            }
            upgradedCards.Clear();
        }

        public override string Name() => "Guidance";
    }
}
