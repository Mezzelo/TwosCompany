using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using TwosCompany.Helper;
using TwosCompany.Cards.Nola;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Ilya;
using Microsoft.Extensions.Logging;
using HarmonyLib;
using System.Reflection;

namespace TwosCompany {
    public partial class Manifest {

        public void BootMod(IModLoaderContact contact) {
            Harmony harmony = new Harmony("Mezz.TwosCompany.Harmony");

            // cost icon card rendering patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.RenderAction)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_StatCostAction_Prefix))
            );

            // card name patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetFullDisplayName)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.DisguisedCardName))
            );

            // move patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AMove), nameof(AMove.Begin)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MoveBegin)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MoveEnd))
            );

            // attack patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AAttack), nameof(AAttack.Begin)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.AttackBegin)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.AttackEnd))
            );

            // missilehit patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AMissileHit), nameof(AMissileHit.Update)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MissileHitBegin)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MissileHitEnd))
            );

            // turn start/end patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnBeginTurn)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.TurnBegin))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Ship), nameof(Ship.OnAfterTurn)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.TurnEnd))
            );

            // play card patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.TryPlayCard)),
                // prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.PlayCardPrefix))
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.PlayCardPostfix))
            );
        }
        private static ICustomEventHub? _eventHub;

        internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

        public static List<ExternalArtifact> Artifacts = new List<ExternalArtifact>();

        public void LoadManifest(ICustomEventHub eventHub) {
            _eventHub = eventHub;

        }



        public void LoadManifest(IArtifactRegistry registry) {
            foreach (ExternalArtifact artifact in Artifacts) {
                registry.RegisterArtifact(artifact);
            }
        }

    }
}
