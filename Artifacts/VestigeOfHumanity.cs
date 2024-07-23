namespace TwosCompany.Artifacts {
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly, ArtifactPool.Unreleased })]
    public class VestigeOfHumanity : Artifact {
        public override string Description() =>
            "At the start of each turn, gain 1 <c=status>ONSLAUGHT</c>.";

        public override void OnTurnStart(State state, Combat combat) {
            combat.Queue(new AStatus() {
                status = (Status)Manifest.Statuses?["Onslaught"].Id!,
                mode = AStatusMode.Add,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = this.Key(),
            });
        }

        public override List<Tooltip>? GetExtraTooltips()
             => new List<Tooltip> { new TTGlossary("status." + Manifest.Statuses?["Onslaught"].Id) };
    }
}