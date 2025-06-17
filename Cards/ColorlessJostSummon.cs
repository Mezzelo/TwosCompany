using TwosCompany.Helper;

namespace TwosCompany.Cards {
    [CardMeta(deck = Deck.colorless, rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class ColorlessJostSummon : Card {
        public override CardData GetData(State state) {
            return new CardData() {
                art = (Spr)Manifest.Sprites["JostDefaultCardSpriteUnsided"].Id!,
                flippable = true,
                cost = upgrade == Upgrade.A ? 0 : 1,
                description = "Get 1 <c=status>" + (flipped ? "off" : "def") + "</c>. "
                    + ColorlessLoc.GetDesc(state, upgrade == Upgrade.B ? 5 : 3, ManifHelper.GetDeck("jost")),
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses[flipped ? "OffensiveStance" : "DefensiveStance"].Id!,
                statusAmount = 1,
                targetPlayer = true,
            });
            actions.Add(new ACardOffering() {
                amount = upgrade == Upgrade.B ? 5 : 3,
                limitDeck = ManifHelper.GetDeck("jost"),
                makeAllCardsTemporary = true,
                overrideUpgradeChances = false,
                canSkip = false,
                inCombat = true,
                discount = -1,
                dialogueSelector = ".summonTCJost"
            });
            return actions;
        }

        public override string Name() => "Jost.EXE";
    }
}
