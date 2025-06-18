using TwosCompany.Helper;

namespace TwosCompany.Cards {
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ColorlessSorrelSummon : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                exhaust = true,
                art = (Spr)Manifest.Sprites["SorrelDefaultCardSprite"].Id!,
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = "Get 1 <c=status>b. time</c>. Add 1 of " + (upgrade == Upgrade.B ? 5 : 3) + " <c=cardtrait>discount, temp</c> "
                    + Manifest.SorrelColH + "Sorrel</c> cards to your hand.",
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses["BulletTime"].Id!,
                statusAmount = 1,
                targetPlayer = true,
            });
            actions.Add(new ACardOffering() {
                amount = upgrade == Upgrade.B ? 5 : 3,
                limitDeck = ManifHelper.GetDeck("sorrel"),
                makeAllCardsTemporary = true,
                overrideUpgradeChances = false,
                canSkip = false,
                inCombat = true,
                discount = -1,
                dialogueSelector = ".summonTCSorrel"
            });
            return actions;
        }

        public override string Name() => "Sorrel.EXE";
    }
}
