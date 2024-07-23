using TwosCompany.Cards.Jost;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class EternalFlame : Artifact {
        public override string Description() =>
            "Start each combat with 2 <c=status>ENFLAMED</c>.";

        public override void OnCombatStart(State state, Combat combat) {
            this.Pulse();
            state.ship.Add((Status)Manifest.Statuses["Enflamed"].Id!, 2 + state.ship.Get(Status.boost));
            state.ship.Set(Status.boost, 0);
        }
        public override List<Tooltip>? GetExtraTooltips()
            => new List<Tooltip> { new TTGlossary("status." + Manifest.Statuses?["Enflamed"].Id, 1) };
    }
}