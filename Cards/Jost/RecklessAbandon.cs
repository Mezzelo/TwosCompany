using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;
using TwosCompany.Cards.Ilya;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class RecklessAbandon : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                exhaust = upgrade != Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            actions.Add(new AStatus() {
                status = (Status) defensiveStance.Id!,
                statusAmount = 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
                dialogueSelector = Stance.Get(s) % 2 != 1 ? null : ".mezz_recklessAbandon",
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AAddCard() {
                    card = new RegainPoise() { upgrade = Upgrade.None, temporaryOverride = true, exhaustOverride = true, discount = -1 },
                    destination = CardDestination.Hand,
                    showCardTraitTooltips = true,
                    disabled = Stance.Get(s) % 2 != 1
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = upgrade == Upgrade.A ? 3 : 1,
                    disabled = Stance.Get(s) % 2 != 1
                });
            

            actions.Add(new ADummyAction());

            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            actions.Add(new AStatus() {
                status = (Status) offensiveStance.Id!,
                statusAmount = 1,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
                dialogueSelector = Stance.Get(s) < 2 ? null : ".mezz_recklessAbandon",
            });
            if (upgrade == Upgrade.A)
                actions.Add(new AAddCard() {
                    card = new RegainPoise() { upgrade = Upgrade.None, temporaryOverride = true, exhaustOverride = true, discount = -1 },
                    destination = CardDestination.Hand,
                    showCardTraitTooltips = true,
                    disabled = Stance.Get(s) < 2
                });
            else if (upgrade == Upgrade.B)
                actions.Add(new ADrawCard() {
                    count = upgrade == Upgrade.A ? 3 : 1,
                    disabled = Stance.Get(s) < 2
                });

            return actions;
        }

        public override string Name() => "Reckless Abandon";
    }
}
