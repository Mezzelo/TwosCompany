using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using TwosCompany.Cards.Ilya;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class Metronome : Artifact, IOnMoveArtifact {
        public int counter = 0;
        public int cardStacks = 0;
        public bool lastWasMove = false;
        public bool consecutive = false;
        public bool fromEvade = false;
        public override string Description() => ManifArtifactHelper.artifactTexts["Metronome"];

        public override int? GetDisplayNumber(State s) => counter;
        public override Spr GetSprite() {
            if (counter == 0)
                return (Spr)(Manifest.Sprites["IconMetronome"].Id
                    ?? throw new Exception("missing artifact art: metronome"));
            if (lastWasMove)
                return (Spr)(Manifest.Sprites["IconMetronomeMoved"].Id
                    ?? throw new Exception("missing artifact art: metronome"));
            else
                return (Spr)(Manifest.Sprites["IconMetronomeAttacked"].Id
                    ?? throw new Exception("missing artifact art: metronome"));
        }

        public override void OnReceiveArtifact(State state) {
            counter = 0;
            cardStacks = 0;
        }

        public override void OnRemoveArtifact(State state) {
            counter = 0;
            cardStacks = 0;
        }
        // public override void OnCombatEnd(State state) => counter = 0;

        public override void OnCombatStart(State state, Combat combat) => cardStacks = 0;

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card playedCard, State state, Combat combat, int handPosition, int handCount) {
            consecutive = false;
            fromEvade = false;
            cardStacks = 0;
        }

        private void Proc(State s, Combat c) {
            counter = 0;
            this.Pulse();
            c.QueueImmediate(new AStatus() {
                targetPlayer = true,
                status = Status.overdrive,
                statusAmount = 1,
                dialogueSelector = ".mezz_metronome",
            });
            c.QueueImmediate(new AStatus() {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = 2
            });
            foreach (CardAction cardAction in c.cardActions) {
                if (cardAction is AAttack aattack && !aattack.targetPlayer && !aattack.fromDroneX.HasValue)
                    aattack.damage += 1 + s.ship.Get(Status.boost);
            }
        }

        public override void OnPlayerAttack(State state, Combat combat) {
            if (!fromEvade) {
                bool markForSet = false;
                if (cardStacks < 2 && (lastWasMove || counter == 0)) {
                    counter++;
                    cardStacks++;
                    markForSet = true;
                    if (counter > 5)
                        Proc(state, combat);
                }
                else if (!consecutive)
                    counter = 0;

                if (markForSet) {
                    lastWasMove = false;
                    consecutive = true;
                }
            }
        }

        public void Movement(int dist, bool targetPlayer, bool fromEvade, Combat c, State s) {
            bool markForSet = false;
            if (!lastWasMove || counter == 0) {
                if (fromEvade || cardStacks < 2) {
                    markForSet = true;
                    counter++;
                    if (!fromEvade)
                        cardStacks++;
                    if (counter > 5)
                        Proc(s, c);
                }
            }
            else if (fromEvade || cardStacks == 0)
                counter = 0;
            if (markForSet) {
                lastWasMove = true;
                this.fromEvade = fromEvade;
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1), new TTGlossary("status.evade", 1) };

    }
}