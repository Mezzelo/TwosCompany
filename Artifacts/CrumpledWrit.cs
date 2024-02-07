using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Media;
using TwosCompany.Cards.Jost;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    public class CrumpledWrit : Artifact {
        public override string Description() => "Start each combat with 1 <c=status>DEF. STANCE</c>. " +
            "When you start your turn with no stance, add an <c=card>Off Balance</c> to your hand if you don't already have one.";

        public bool discountOffBalance = false;
        public override void OnReceiveArtifact(State state) => discountOffBalance = state.EnumerateAllArtifacts().OfType<BurdenOfHindsight>().Any();

        public override void OnCombatStart(State state, Combat combat) {
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            combat.Queue(new AStatus() {
                status = (Status) defensiveStance.Id!,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = this.Key(),
            });
        }
        public override void OnTurnStart(State state, Combat combat) {
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            ExternalStatus footwork = Manifest.Statuses?["Footwork"] ?? throw new Exception("status missing: footwork");
            if (state.ship.Get((Status)defensiveStance.Id!) + state.ship.Get((Status)offensiveStance.Id!) == 0 &&
                state.ship.Get((Status) footwork.Id!) <= 0) {
                bool found = false;
                foreach (Card card in combat.hand)
                    if (card is OffBalance) {
                        found = true;
                        break;
                    }
                if (!found)
                    combat.Queue(new AAddCard() {
                        card = new OffBalance() { discount = (discountOffBalance ? -1 : 0) },
                        destination = CardDestination.Hand,
                        artifactPulse = this.Key(),
                        dialogueSelector = ".mezz_cannonGuard",
                    });
            }
        }

        public override List<Tooltip>? GetExtraTooltips() => new List<Tooltip>() { 
            // new TTGlossary("status.DefensiveStance", 1),
            new TTCard() {
                card = new OffBalance(),
            }
        };
    }
}