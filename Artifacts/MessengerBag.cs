using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Media;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class MessengerBag : Artifact {
        public int heartbeats = 0;
        public override int? GetDisplayNumber(State s) => heartbeats;
        public override string Description() => "On pickup, gain 2 <c=card>Heartbeats</c>. " +
            "The first 2 times you draw a <c=card>Heartbeat</c> each turn, gain 1 <c=status>ONSLAUGHT</c>.";

        public override void OnReceiveArtifact(State state) {
            state.GetCurrentQueue().Insert(0, new AAddCard() {
                card = new Heartbeat(),
                amount = 2,
            });
        }
        public override void OnTurnEnd(State state, Combat combat) => heartbeats = 0;
        public override void OnCombatEnd(State state) => heartbeats = 0;
        public override List<Tooltip>? GetExtraTooltips() {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTCard() {
                card = new Heartbeat(),
            });
            list.Add(new TTGlossary("status." + Manifest.Statuses?["Onslaught"].Id));
            return list;
        }
    }
}