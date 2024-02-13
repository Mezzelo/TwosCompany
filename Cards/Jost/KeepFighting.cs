using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class KeepFighting : Card, IJostCard {

        public bool isTemp = false;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 2,
                temporary = isTemp,
                exhaust = isTemp && upgrade == Upgrade.A,
                recycle = !(isTemp && upgrade == Upgrade.A),
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            int disc = -1;
            if (s.route is Combat route)
                disc = s.ship.Get(Status.temporaryCheap) > 0 ? 0 : -1;
           

            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = 2,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
                dialogueSelector = Stance.Get(s) % 2 != 1 || this.discount > -1 ? null : ".mezz_keepFighting",
            });
            actions.Add(new AAddCard() {
                card = new KeepFighting() { isTemp = true, upgrade = (this.upgrade == Upgrade.A ? Upgrade.A : Upgrade.None), discount = disc },
                destination = CardDestination.Hand,
                showCardTraitTooltips = false,
                amount = upgrade == Upgrade.B ? 2 : 1,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new ADummyAction());

            actions.Add(new AAttack() {
                damage = GetDmg(s, 3),
                disabled = Stance.Get(s) < 2,
                dialogueSelector = Stance.Get(s) < 2 || this.discount > -1 ? null : ".mezz_keepFighting",
            });
            actions.Add(new AAddCard() {
                card = new KeepFighting() { isTemp = true, upgrade = (this.upgrade == Upgrade.A ? Upgrade.A : Upgrade.None), discount = disc },
                destination = CardDestination.Hand,
                showCardTraitTooltips = false,
                amount = upgrade == Upgrade.B ? 2 : 1,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }

        public override string Name() => "Keep Fighting";
    }
}
