using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;
using TwosCompany.Cards.Ilya;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class Heartbeat : Card, IJostCard {
        public override CardData GetData(State state) {
            return new CardData() {
                cost = upgrade == Upgrade.B ? 1 : 0,
                infinite = upgrade == Upgrade.B,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade != Upgrade.B ? "Up1" : "") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.B ? 2 : 1,
                disabled = Stance.Get(s) % 2 != 1
            });

            actions.Add(new ADummyAction());

            actions.Add(new ADrawCard() {
                count = upgrade == Upgrade.B ? 2 : 1,
                disabled = Stance.Get(s) < 2,
            });
            if (upgrade != Upgrade.B) {
                
                    ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensiveStance");
                    actions.Add(new StatCostAction() {
                        action = (upgrade == Upgrade.A ? 
                            new ADrawCard() {
                                count = 1,
                                disabled = Stance.Get(s) < 2,
                            } :
                            new AEnergy() {
                                changeAmount = -1,
                                disabled = Stance.Get(s) < 2,
                            }),
                        statusReq = defensiveStance.Id != null ? (Status) defensiveStance.Id : Status.overdrive,
                        statusCost = 1,
                        first = true,
                        disabled = Stance.Get(s) < 2,
                    });

            }
            return actions;
        }

        public override string Name() => "Heartbeat";
    }
}
