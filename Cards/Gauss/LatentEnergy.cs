using TwosCompany.Actions;
using TwosCompany.Cards.Ilya;

namespace TwosCompany.Cards.Gauss {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LatentEnergy : Card {

        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["LatentEnergy"].DescLocKey ?? throw new Exception("Missing card description")));
            else if (upgrade == Upgrade.A)
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["LatentEnergy"].DescALocKey ?? throw new Exception("Missing card description")));
            else
                cardText = String.Format(Loc.GetLocString(Manifest.Cards?["LatentEnergy"].DescBLocKey ?? throw new Exception("Missing card description")));

            return new CardData() {
                cost = 1,
                description = cardText,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AAddCard() {
                amount = 1,
                card = new SparkCard() { upgrade = this.upgrade, temporaryOverride = true, discount = -1 },
                destination = CardDestination.Hand,
            });
            return actions;
        }

        public override string Name() => "Latent Energy";
    }
}
