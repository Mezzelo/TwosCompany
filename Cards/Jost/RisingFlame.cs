using CobaltCoreModding.Definitions.ExternalItems;
using TwosCompany.Actions;

namespace TwosCompany.Cards.Jost {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B }, extraGlossary = new string[] { "action.StanceCard" })]
    public class RisingFlame : Card, IJostCard {

        public int costIncrease = 1;
        public override CardData GetData(State state) {
            return new CardData() {
                cost = 1,
                infinite = upgrade == Upgrade.A,
                exhaust = false,
                art = new Spr?((Spr)(Manifest.Sprites["JostDefaultCardSprite" + (upgrade == Upgrade.B ? "" : "Up1") + Stance.AppendName(state)].Id
                    ?? throw new Exception("missing card art")))
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            actions.Add(new AAllIncreaseHint() {
                amount = upgrade == Upgrade.B ? 1 : 2,
                isCombat = true,
                disabled = Stance.Get(s) % 2 != 1
            });
            actions.Add(new ACostDecreasePlayedHint() {
                amount = costIncrease + (upgrade == Upgrade.B ? 0 : 1),
                disabled = Stance.Get(s) % 2 != 1
            });

            if (upgrade == Upgrade.B) {
                actions.Add(new ADummyTooltip() {
                    action = Stance.Get(s) < 2 ? new AAddCard() {
                        card = new HackAndSlash(),
                        destination = CardDestination.Hand,
                        amount = costIncrease + (Stance.Get(s) == 3 ? 2 : 0) + 1,
                        disabled = Stance.Get(s) < 2,
                    } : null,
                });
                actions.Add(new AAddCard() {
                    card = new HackAndSlash(),
                    destination = CardDestination.Hand,
                    amount = costIncrease + (Stance.Get(s) == 3 ? 2 : 0) + 1,
                    disabled = Stance.Get(s) < 2,
                });
            } else {
                actions.Add(new AAttack() {
                    damage = GetDmg(s, costIncrease + (Stance.Get(s) == 3 ? 2 : 0)),
                    fast = true,
                    disabled = Stance.Get(s) < 2,
                    dialogueSelector = Stance.Get(s) < 2 || costIncrease == 1 ? null : ".mezz_risingFlame",
                });
                actions.Add(new AAttack() {
                    damage = GetDmg(s, costIncrease + (Stance.Get(s) == 3 ? 2 : 0)),
                    fast = true,
                    disabled = Stance.Get(s) < 2,
                });
            }
            actions.Add(new AExhaustSelf() {
                uuid = this.uuid,
                omitFromTooltips = false,
                disabled = Stance.Get(s) < 2,
            });
            return actions;
        }
        public override void OnExitCombat(State s, Combat c) {
            costIncrease = 1;
        }

        public override void AfterWasPlayed(State state, Combat c) {
            if (Stance.Get(state) % 2 == 1) {
                costIncrease += upgrade == Upgrade.B ? 1 : 2;
                // this.discount += 1;
            }
            // wasPlayed = true;
        }

        public override string Name() => "Reactive Defense";
    }
}
