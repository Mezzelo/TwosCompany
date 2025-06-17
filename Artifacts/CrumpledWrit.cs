using CobaltCoreModding.Definitions.ExternalItems;
using Microsoft.Xna.Framework.Media;
using System.Linq;
using TwosCompany.Cards.Jost;
using TwosCompany.Helper;

namespace TwosCompany.Artifacts {

    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    public class CrumpledWrit : Artifact {
        public override string Description() => ManifArtifactHelper.artifactTexts["CrumpledWrit"];

        public bool discountOffBalance = false;
        public bool hasVengeance = false;
        public override void OnReceiveArtifact(State state) {
            discountOffBalance = state.EnumerateAllArtifacts().OfType<BurdenOfHindsight>().Any();
            hasVengeance = state.EnumerateAllArtifacts().OfType<AimlessVengeance>().Any();
        }

        public override void OnCombatStart(State state, Combat combat) {
            if (hasVengeance)
                return;
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            combat.Queue(new AStatus() {
                status = (Status) defensiveStance.Id!,
                statusAmount = 1,
                targetPlayer = true,
                artifactPulse = this.Key(),
            });
        }
        public override void OnTurnStart(State state, Combat combat) {
            if (hasVengeance)
                return;
            ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
            ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
            ExternalStatus footwork = Manifest.Statuses?["Footwork"] ?? throw new Exception("status missing: footwork");
            if (state.ship.Get((Status)defensiveStance.Id!) + state.ship.Get((Status)offensiveStance.Id!) == 0 &&
                state.ship.Get((Status) footwork.Id!) <= 0) {
                if (!combat.hand.Any((Card c) => c is OffBalance))
                    combat.Queue(new AAddCard() {
                        card = new OffBalance() { discount = (discountOffBalance ? -1 : 0) },
                        destination = CardDestination.Hand,
                        artifactPulse = this.Key(),
                        dialogueSelector = ".mezz_crumpledWrit",
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