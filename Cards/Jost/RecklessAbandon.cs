using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;
using TwosCompany.Cards.Ilya;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class RecklessAbandon : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 2 : 1,
                retain = upgrade != Upgrade.B,
                exhaust = upgrade != Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();
            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?[upgrade == Upgrade.B ? "OffensiveStance" : "DefensiveStance"].Id!,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
                dialogueSelector = Stance.Get(s) == 1 ? ".mezz_recklessAbandon" : null,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = (Status) Manifest.Statuses?["StandFirm"].Id!,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) % 2 != 1,
                });


            actions.Add(new ADummyAction());

            actions.Add(new AStatus() {
                status = (Status) Manifest.Statuses?[upgrade == Upgrade.B ? "DefensiveStance" : "OffensiveStance"].Id!,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true,
                disabled = Stance.Get(s) < 2,
                dialogueSelector = Stance.Get(s) == 2 ? ".mezz_recklessAbandon" : null,
            });
            if (upgrade == Upgrade.B)
                actions.Add(new AStatus() {
                    status = (Status) Manifest.Statuses?["StandFirm"].Id!,
                    statusAmount = 1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) < 2,
                });

            return actions;
        }

        public override string Name() => "Reckless Abandon";
    }
}
