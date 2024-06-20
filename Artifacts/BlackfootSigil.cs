using System.Diagnostics.Metrics;
using System.Reflection;
using TwosCompany.Actions;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class BlackfootSigil : Artifact {
        public override string Description() => "At the start of combat, gain a <c=card>Freedom of Movement</c>.";

        public override void OnCombatStart(State state, Combat combat) => combat.QueueImmediate(new AAddCard() {
            card = new FreedomOfMovement(),
            destination = CardDestination.Hand,
            amount = 1
        });

        public override List<Tooltip>? GetExtraTooltips() {
            List<Tooltip> extraTooltips = new List<Tooltip>();
            extraTooltips.Add((Tooltip)new TTCard() {
                card = (Card)new FreedomOfMovement()
            });
            return extraTooltips;
        }
    }

}