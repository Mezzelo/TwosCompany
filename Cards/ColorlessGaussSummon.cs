using TwosCompany.Cards.Gauss;
using TwosCompany.Helper;

namespace TwosCompany.Cards {
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ColorlessGaussSummon : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                exhaust = true,
                art = (Spr)Manifest.Sprites["GaussDefaultCardSprite"].Id!,
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = "Add a <c=cardtrait>temp</c> <c=card>Spark</c> & 1 of " +
                    (upgrade == Upgrade.B ? "5" : "3") + " <c=cardtrait>discount, temp</c> " + Manifest.GaussColH + "Gauss</c> cards to your hand.",
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAddCard() {
                amount = 1,
                card = new SparkCard() { upgrade = Upgrade.None, temporaryOverride = true },
                destination = upgrade == Upgrade.A ? CardDestination.Hand : CardDestination.Hand,
            });

            actions.Add(new ACardOffering() {
                amount = upgrade == Upgrade.B ? 5 : 3,
                limitDeck = ManifHelper.GetDeck("gauss"),
                makeAllCardsTemporary = true,
                overrideUpgradeChances = false,
                canSkip = false,
                inCombat = true,
                discount = -1,
                dialogueSelector = ".summonTCGauss"
            });
            return actions;
        }

        public override string Name() => "Gauss.EXE";
    }
}
