using TwosCompany.Helper;

namespace TwosCompany.Cards {
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ColorlessIsabelleSummon : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                art = (Spr)Manifest.Sprites["IsabelleDefaultCardSprite"].Id!,
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 5 : 3, ManifHelper.GetDeck("isabelle")),
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ACardOffering() {
                amount = upgrade == Upgrade.B ? 5 : 3,
                limitDeck = ManifHelper.GetDeck("isabelle"),
                makeAllCardsTemporary = true,
                overrideUpgradeChances = false,
                canSkip = false,
                inCombat = true,
                discount = -1,
                dialogueSelector = ".summonTCIsabelle"
            });
            return actions;
        }

        public override string Name() => "Isabelle.EXE";
    }
}
