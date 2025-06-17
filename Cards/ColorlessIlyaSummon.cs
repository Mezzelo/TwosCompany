using TwosCompany.Helper;

namespace TwosCompany.Cards {
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ColorlessIlyaSummon : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                art = (Spr)Manifest.Sprites["IlyaDefaultCardSprite"].Id!,
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = "Get 2 <c=status>heat</c>. "
                    + ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 5 : 3, ManifHelper.GetDeck("ilya")),
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.heat,
                statusAmount = 2,
                targetPlayer = true,
            });
            actions.Add(new ACardOffering() {
                amount = upgrade == Upgrade.B ? 5 : 3,
                limitDeck = ManifHelper.GetDeck("ilya"),
                makeAllCardsTemporary = true,
                overrideUpgradeChances = false,
                canSkip = false,
                inCombat = true,
                discount = -1,
                dialogueSelector = ".summonTCIlya"
            });
            return actions;
        }

        public override string Name() => "Ilya.EXE";
    }
}
