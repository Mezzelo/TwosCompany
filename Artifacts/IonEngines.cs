using TwosCompany.Helper;

namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class IonEngines : Artifact {
        public int counter = 0;

        public override string Description() => ManifArtifactHelper.artifactTexts["IonEngines"];
        public override int? GetDisplayNumber(State s) => counter;

        public IonEngines() => 
            Manifest.EventHub.ConnectToEvent<Tuple<State, int>>("Mezz.TwosCompany.ChainLightning", Chain);

        public override void OnReceiveArtifact(State state) {
            Manifest.EventHub.ConnectToEvent<Tuple<State, int>>("Mezz.TwosCompany.ChainLightning", Chain);
            counter = 0;
        }

        public override void OnRemoveArtifact(State state) {
            Manifest.EventHub.DisconnectFromEvent<Tuple<State, int>>("Mezz.TwosCompany.ChainLightning", Chain);
            counter = 0;
        }

        private void Chain(Tuple<State, int> evt) {
            State s = evt.Item1;
            int distance = evt.Item2;

            if (!s.characters.SelectMany(e => e.artifacts).Concat(s.artifacts).Contains(this)) {
                Manifest.EventHub.DisconnectFromEvent<Tuple<State, int>>("Mezz.TwosCompany.ChainLightning", Chain);
                return;
            }

            counter += distance;
            while (counter > 5) {
                counter -= 5;
                this.Pulse();
               ((Combat) s.route).QueueImmediate(new AStatus() {
                   targetPlayer = true,
                   status = Status.evade,
                   statusAmount = 1
               });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() {
            // new TTGlossary(Manifest.Glossary["ChainLightning"]!.Head, 1),
            new TTGlossary("status.evade", 1)
        };
    }
}