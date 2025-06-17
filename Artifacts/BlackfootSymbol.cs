using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;
using TwosCompany.Midrow;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class BlackfootSymbol : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["BlackfootSymbol"];
        public override void OnTurnStart(State state, Combat combat) {
            if (state.ship.Get(Status.droneShift) <= 2 && combat.stuff.Values.Where(stuff => stuff is FrozenAttack).Count() > 0) {
                combat.Queue(new AStatus() {
                    status = Status.droneShift,
                    mode = AStatusMode.Add,
                    statusAmount = 1,
                    targetPlayer = true,
                    artifactPulse = this.Key(),
                    dialogueSelector = ".mezz_blackfootSymbol",
                });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { new TTGlossary("status.droneShift") };
    }

}