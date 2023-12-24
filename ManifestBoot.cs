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
        private static ICustomEventHub? _eventHub;
        internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

        public void LoadManifest(ICustomEventHub eventHub) {
            _eventHub = eventHub;
            //distance, target_player, from_evade, combat, state
            eventHub.MakeEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement");
            Harmony harmony = new Harmony("Mezz.TwosCompany.Harmony");

        }



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

            // table flip patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetDataWithOverrides)),
                // prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.PlayCardPrefix))
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.CardDataPostfix))
            );

            // patch strings manually
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(DB), nameof(DB.LoadStringsForLocale)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.LocalePostfix))
            );

            // dialogue patches
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AI), nameof(AI.OnCombatStart)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.AICombatStartPrefix))
            );

            // i gotta do this manually for all overridden start methods oh the misery.
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(OxygenLeakGuy), nameof(OxygenLeakGuy.OnCombatStart)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.OxygenLeakGuyCombatStartPrefix))
            );




        }

        public void LoadManifest(IArtifactRegistry registry) {
            for (int i = 0; i < ManifArtifactHelper.artifactNames.Length; i++) {
                string artifact = ManifArtifactHelper.artifactNames[i];
                if (Type.GetType("TwosCompany.Artifacts." + artifact) == null)
                    continue;
                Artifacts.Add(artifact, new ExternalArtifact("TwosCompany.Artifacts." + artifact,
                    Type.GetType("TwosCompany.Artifacts." + artifact) ?? throw new Exception("artifact type not found: " + artifact),
                    Sprites["Icon" + artifact] ?? throw new Exception("missing MidrowProtectorProtocol sprite"),
                    ownerDeck: (i < 5 ? NolaDeck : (i < 10 ? IsabelleDeck : IlyaDeck))));
                Artifacts[artifact].AddLocalisation(ManifArtifactHelper.artifactLocs[i].ToUpper(), ManifArtifactHelper.artifactTexts[i]);
                registry.RegisterArtifact(Artifacts[artifact]);
            }
        }

    }
}
