using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Commit : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                exhaust = upgrade != Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AStatus() {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.A ? 4 : 6,
                targetPlayer = true,
                disabled = Stance.Get(s) % 2 != 1,
                dialogueSelector = Stance.Get(s) % 2 != 1 ? null : ".mezz_commit",
            });
            if (upgrade != Upgrade.A) {
                ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
                actions.Add(new AStatus() {
                    status = (Status)defensiveStance.Id!,
                    statusAmount = -1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) % 2 != 1,
                });
            }

            actions.Add(new ADummyAction());

            actions.Add(new AAttack() {
                damage = GetDmg(s, upgrade == Upgrade.A ? 4 : 6),
                disabled = Stance.Get(s) < 2,
                dialogueSelector = Stance.Get(s) < 2 ? null : ".mezz_commit",
            });
            if (upgrade != Upgrade.A) {
                ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
                actions.Add(new AStatus() {
                    status = (Status)offensiveStance.Id!,
                    statusAmount = -1,
                    targetPlayer = true,
                    disabled = Stance.Get(s) < 2,
                });
            }
            return actions;
        }

        public override string Name() => "Commit";
    }
}
