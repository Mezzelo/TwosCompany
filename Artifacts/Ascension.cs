using TwosCompany.Cards.Nola;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;
using TwosCompany.Cards.Gauss;
using TwosCompany.Cards.Sorrel;
using TwosCompany.Helper;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class Ascension : Artifact {

        public static List<Card>[] cardLists = new List<Card>[] {
            new List<Card> {
                new BlastShield() { discount = -1 },
                new DragonsBreath() { upgrade = Upgrade.B, discount = -1 },
            },
            new List<Card> {
                new AsteroidBelt() { upgrade = Upgrade.A, discount = -1 },
                new Electrocute() { upgrade = Upgrade.A, discount = -1 },
                new KineticConduit() { upgrade = Upgrade.B, discount = -1 },
            },
            new List<Card> {
                new Sideswipe() { upgrade = Upgrade.A , discount = -1 },
                new Cascade() { upgrade = Upgrade.A , discount = -1 },
            },
            new List<Card> {
                new ReactiveDefense() { upgrade = Upgrade.A, discount = -1 },
                new OverheadBlow() { upgrade = Upgrade.B, discount = -1 },
            },
            new List<Card> {
                new Foresight() { upgrade = Upgrade.A, discount = -1 },
                new Relentless() { discount = -1 },
            },
        };

        public List<Card> cardQueue = new List<Card>();

        public int counter = 0;
        public int characters = 0;
        public override string Description() => ManifArtifactHelper.artifactTexts["Ascension"];
        public override int? GetDisplayNumber(State s) => counter > 3 + characters * 2 || counter < 1 ? null : counter;
        public override Spr GetSprite() {
            if (characters == 0)
                return (Spr)(Manifest.Sprites["IconAscension"].Id
                    ?? throw new Exception("missing artifact art: ascension"));
            else
                return (Spr)(Manifest.Sprites["IconAscension_" + characters].Id
                    ?? throw new Exception("missing artifact art: ascension"));
        }


        public override void OnTurnStart(State state, Combat combat) {
            counter++;
            if (counter == 4 + characters * 2) {
                combat.Queue(new AAddCard() {
                    card = new Karma() { upgrade = Upgrade.B, retainOverride = true, exhaustOverride = false, storyInf = true },
                    destination = CardDestination.Hand,
                    amount = 1,
                    artifactPulse = this.Key(),
                });
            } else if (characters > 0) {
                bool jostDrawn = false;
                for (int g = 0; g < Math.Ceiling(characters / 2.0); g++) {
                    if (cardQueue.Count == 0) {
                        for (int i = 0; i < characters; i++) {
                            foreach (Card c in cardLists[i]) {
                                Card c2 = c.CopyWithNewId();
                                c2.temporaryOverride = true;
                                c2.exhaustOverride = true;
                                c2.exhaustOverrideIsPermanent = true;
                                cardQueue.Add(c2);
                            }
                        }
                        cardQueue = cardQueue.OrderBy(_ => Guid.NewGuid()).ToList();
                    }
                    combat.Queue(new AAddCard() {
                        card = cardQueue[0],
                        destination = CardDestination.Hand,
                        amount = 1,
                    });
                    if (!jostDrawn && cardQueue[0].GetMeta().deck.Equals(ManifHelper.GetDeck("jost")) &&
                        !combat.hand.Any((Card c) => c is RecklessAbandon)) {
                        jostDrawn = true;
                        combat.Queue(new AAddCard() {
                            card = new RecklessAbandon() {
                                upgrade = Upgrade.A,
                                temporaryOverride = true,
                                discount = -1,
                            },
                            destination = CardDestination.Hand,
                            amount = 1,
                        });
                    }
                    cardQueue.RemoveAt(0);
                }
            }
        }

        public override void OnCombatEnd(State state) {
            cardQueue.Clear();
            counter = 0;
        }

        public override List<Tooltip>? GetExtraTooltips() {
            List<Tooltip> extraTooltips = new List<Tooltip>();
            extraTooltips.Add(new TTCard() {
                card = new Karma() { upgrade = Upgrade.B, retainOverride = true, exhaustOverride = false, storyInf = true }
            });
            return extraTooltips;
        }
    }
}