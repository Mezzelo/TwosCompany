using TwosCompany.Helper;

namespace TwosCompany.Cards
{
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ColorlessNolaSummon : Card
    {
        public override CardData GetData(State state) {
            return new CardData() {
                art = (Spr)Manifest.Sprites["NolaDefaultCardSprite"].Id!,
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 5 : 3, ManifHelper.GetDeck("nola")),
            };
        }

        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ACardOffering()
            {
                amount = upgrade == Upgrade.B ? 5 : 3,
                limitDeck = ManifHelper.GetDeck("nola"),
                makeAllCardsTemporary = true,
                overrideUpgradeChances = false,
                canSkip = false,
                inCombat = true,
                discount = -1,
                dialogueSelector = ".summonTCNola"
            });
            return actions;
        }

        public override string Name() => "Nola.EXE";
    }
}
