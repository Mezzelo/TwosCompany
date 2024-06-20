using TwosCompany.Actions;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class Feedback : Card {

        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                flippable = upgrade == Upgrade.B,
                art = new Spr?((Spr)((flipped ? Manifest.Sprites["FeedbackCardSprite"] : Manifest.Sprites["FeedbackCardSprite"]).Id
                    ?? throw new Exception("missing flip art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new ASpawn() {
                thing = new Midrow.Conduit() {
                    condType = Midrow.Conduit.ConduitType.feedback,
                    bubbleShield = upgrade == Upgrade.A,
                },
                offset = upgrade == Upgrade.B ? 1 : 0,
                dialogueSelector = ".mezz_feedback",
            });
            if (upgrade == Upgrade.B)
                actions.Add(new ASpawn() {
                    thing = new Midrow.Conduit() {
                        condType = Midrow.Conduit.ConduitType.normal,
                    },
                });
            return actions;
        }

        public override string Name() => "Feedback Conduit";
    }
}
