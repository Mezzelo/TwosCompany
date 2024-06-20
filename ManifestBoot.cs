using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using TwosCompany.Helper;
using HarmonyLib;
using TwosCompany.ExternalAPI;
using Microsoft.Extensions.Logging;
using TwosCompany.Actions;

namespace TwosCompany {
    public partial class Manifest : IStoryManifest {
        private static ICustomEventHub? _eventHub;
        internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

        public void LoadManifest(ICustomEventHub eventHub) {
            _eventHub = eventHub;
            //distance, target_player, from_evade, combat, state
            eventHub.MakeEvent<Tuple<int, bool, bool, Combat, State>>("Mezz.TwosCompany.Movement");
            eventHub.MakeEvent<Tuple<State, Combat>>("Mezz.TwosCompany.StanceSwitch");
            eventHub.MakeEvent<Tuple<State, int>>("Mezz.TwosCompany.ChainLightning");

            eventHub.ConnectToEvent<Func<IManifest, IPrelaunchContactPoint>>("Nickel::OnAfterDbInitPhaseFinished", contactPointProvider => {
                var contactPoint = contactPointProvider(this);

                if (contactPoint.GetApi<IAppleShipyardApi>("APurpleApple.Shipyard") is { } apurpleappleApi) {
                    apurpleappleApi.RegisterActionLooksForPartType(typeof(AChainLightning), PType.missiles);
                }

                if (contactPoint.GetApi<IDraculaApi>("Shockah.Dracula") is { } draculaApi) {
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["TempStrafe"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["MobileDefense"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Onslaught"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 3 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["FollowUp"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 2 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["FalseOpening"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 4 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["FalseOpeningB"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 2 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Enflamed"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["DefensiveStance"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["OffensiveStance"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["StandFirm"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Footwork"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["BattleTempo"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["DistantStrike"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 3 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["ElectrocuteCharge"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 2 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["ElectrocuteChargeSpent"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 2 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Autocurrent"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["HyperspaceStorm"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["HyperspaceStormA"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["HyperspaceStormB"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Control"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Superposition"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                }
            });
        }

        public object? GetApi(IManifest requestingMod)
            => new APIImplementation();

        public void BootMod(IModLoaderContact contact) {

            Instance = this;
            Harmony harmony = new Harmony("Mezz.TwosCompany.Harmony");
            if (contact.LoadedManifests.Any(manifest => manifest.Name == "TheJazMaster.MoreDifficulties"))
                MoreDifficultiesApi = contact.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");

            // midrow chain lightning rendering patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.RenderDrones)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RenderDronesPrefix))
            );
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.RenderHintsUnderlay)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RenderHintsUnderlayPrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(ADroneMove), nameof(ADroneMove.Begin)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.ADroneMovePrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DestroyDroneAt)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.DestroyDroneAtPrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.ResetHilights)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.ResetHilightsPrefix))
            );

            // cost icon card rendering patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.RenderAction)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_StatCostAction_Prefix))
            );

            // card ondraw patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.SendCardToHand)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_OnDraw_Postfix))
            );

            /*
            // draincardactions patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DrainCardActions)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_DrainCardActions_Prefix))
            );

            // begincardactions patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), "BeginCardAction"),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_BeginCardAction_Postfix))
            ); */

            // card flip patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.FlipCardInHand)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_FlipCardInHand_Postfix))
            );

            // card damage patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Card), nameof(Card.GetActualDamage)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_GetActualDamage_Postfix))
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
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.PlayCardPrefix)),
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

            // dialogue sayswitch injection patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(MG), "DrawLoadingScreen"),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.DialogueInjectionPrefix)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.DialogueInjectionPostfix))
            );

            // story music patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Dialogue), nameof(Dialogue.GetMusic)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.DialogueMusicPostfix))
            );

            // other dialogue patches
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(AI), nameof(AI.OnCombatStart)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.AICombatStartPrefix))
            );

            // i gotta do this manually for all overridden start methods oh the misery.
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(OxygenLeakGuy), nameof(OxygenLeakGuy.OnCombatStart)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.OxygenLeakGuyCombatStartPrefix))
            );

            // give jost starter artifact after getting him from event, or getting his foreign card event
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Events), nameof(Events.CrystallizedFriendEvent)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.CrystallizedFriendPostfix))
            );
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Events), nameof(Events.ForeignCardOffering)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.ForeignCardOfferingPostfix))
            );

            // unfortunately can't fix run history colors without transpiler, but this does solve some other cases.
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Colors), nameof(Colors.LookupColor)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.LookupColorPrefix))
            );


        }

        public void LoadManifest(IArtifactRegistry registry) {
            for (int i = 0; i < ManifArtifactHelper.artifactNames.Keys.Count; i++) {
                string artifact = ManifArtifactHelper.artifactNames.ElementAt(i).Key;
                if (Type.GetType("TwosCompany.Artifacts." + artifact) == null)
                    continue;
                if (NolaDeck == null || IsabelleDeck == null || IlyaDeck == null || JostDeck == null || GaussDeck == null)
                    throw new Exception("deck not initialized in artif registry");
                ExternalDeck pickDeck = GaussDeck;
                if (i < 6)
                    pickDeck = NolaDeck;
                else if (i < 12)
                    pickDeck = IsabelleDeck;
                else if (i < 18)
                    pickDeck = IlyaDeck;
                else if (i < 25)
                    pickDeck = JostDeck;

                Artifacts.Add(artifact, new ExternalArtifact("TwosCompany.Artifacts." + artifact,
                    Type.GetType("TwosCompany.Artifacts." + artifact) ?? throw new Exception("artifact type not found: " + artifact),
                    Sprites["Icon" + artifact] ?? throw new Exception("missing MidrowProtectorProtocol sprite"),
                    ownerDeck: pickDeck));
                Artifacts[artifact].AddLocalisation(ManifArtifactHelper.artifactNames.ElementAt(i).Value.ToUpper(), ManifArtifactHelper.artifactTexts[i]);
                registry.RegisterArtifact(Artifacts[artifact]);
            }
        }

    }
}
