using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using TwosCompany.Cards.Ilya;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class Metronome : Artifact {
        public int counter = 0;
        public bool lastWasMove = false;
        public bool consecutive = false;
        public bool fromStrafe = false;
        public override string Description() => ManifArtifactHelper.artifactTexts["Metronome"];

        public override int? GetDisplayNumber(State s) => counter;

        public Metronome() =>
            Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
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
            Manifest.EventHub.ConnectToEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
            counter = 0;
        }

        public override void OnRemoveArtifact(State state) {
            Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
            counter = 0;
        }
        // public override void OnCombatEnd(State state) => counter = 0;

        public override void OnPlayerPlayCard(int energyCost, Deck deck, Card playedCard, State state, Combat combat, int handPosition, int handCount) {
            consecutive = false;
            fromStrafe = false;

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
            if (!fromStrafe) {
                if (lastWasMove || counter == 0) {
                    counter++;
                    if (counter > 5)
                        Proc(state, combat);
                }
                else if (!consecutive)
                    counter = 0;
                lastWasMove = false;
                consecutive = true;
            }
        }

        private void Movement(Tuple<int, bool, bool, Combat, State> evt) {
            int distance = evt.Item1;
            Combat c = evt.Item4;
            State s = evt.Item5;

            if (!s.characters.SelectMany(e => e.artifacts).Concat(s.artifacts).Contains(this)) {
                Manifest.EventHub.DisconnectFromEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement", Movement);
                return;
            }

            if (!lastWasMove || counter == 0) {
                counter++;
                if (counter > 5)
                    Proc(s, c);
            }
            else
                counter = 0;
            lastWasMove = true;
            fromStrafe = evt.Item3;
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.overdrive", 1), new TTGlossary("status.evade", 1) };
    }
}