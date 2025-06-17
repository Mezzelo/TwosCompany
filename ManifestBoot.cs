using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using TwosCompany.Helper;
using HarmonyLib;
using TwosCompany.ExternalAPI;
using Microsoft.Extensions.Logging;
using TwosCompany.Actions;
using Microsoft.Extensions.Logging.Abstractions;
using System.Reflection;
using Nickel.Legacy;
using Nanoray.PluginManager;
using Nickel;
using Shockah.Kokoro;

namespace TwosCompany {
    public partial class Manifest : IStoryManifest, INickelManifest {
        private static ICustomEventHub? _eventHub;
        internal static ICustomEventHub EventHub { get => _eventHub ?? throw new Exception(); set => _eventHub = value; }

        public void OnNickelLoad(IPluginPackage<Nickel.IModManifest> package, IModHelper helper) {
            hasNickel = true;
            this.Helper = helper;
            settings = helper.Storage.LoadJson<ModSettings>(helper.Storage.GetMainStorageFile("json"));

            
            Traits.Add("EnergyPerAttack", helper.Content.Cards.RegisterTrait("EnergyPerAttack", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconEnergyPerAttack"].Id!.Value,
                Name = _ => "Patient",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["EnergyPerAttack"]?.Head!, 1)]
            }));
            Traits.Add("EnergyPerAttackIncrease", helper.Content.Cards.RegisterTrait("EnergyPerAttackIncrease", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconEnergyPerAttackIncrease"].Id!.Value,
                Name = _ => "Impatient",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["EnergyPerAttackIncrease"]?.Head!, 1)]
            }));
            Traits.Add("EnergyPerCard", helper.Content.Cards.RegisterTrait("EnergyPerCard", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconEnergyPerCard"].Id!.Value,
                Name = _ => "Urgent",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["EnergyPerCard"]?.Head!, 1)]
            }));
            Traits.Add("EnergyPerPlay", helper.Content.Cards.RegisterTrait("EnergyPerPlay", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconEnergyPerPlay"].Id!.Value,
                Name = _ => "Rising Cost",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["EnergyPerPlay"]?.Head!, 1)]
            }));
            Traits.Add("AllIncrease", helper.Content.Cards.RegisterTrait("AllIncrease", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconAllIncrease"].Id!.Value,
                Name = _ => "Intensify",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["AllIncrease"]?.Head!, 1)]
            }));
            Traits.Add("AllIncreaseCombat", helper.Content.Cards.RegisterTrait("AllIncreaseCombat", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconAllIncreaseCombat"].Id!.Value,
                Name = _ => "Lasting Intensify",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["AllIncreaseCombat"]?.Head!, 1)]
            }));
            Traits.Add("TurnIncreaseCost", helper.Content.Cards.RegisterTrait("TurnIncreaseCost", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconTurnIncreaseCost"].Id!.Value,
                Name = _ => "Timed Cost",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["TurnIncreaseCost"]?.Head!, 1)]
            }));
            Traits.Add("DisguisedHint", helper.Content.Cards.RegisterTrait("DisguisedHint", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconDisguisedHint"].Id!.Value,
                Name = _ => "Disguised",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["DisguisedHint"]?.Head!)]
            }));
            Traits.Add("DisguisedPermaHint", helper.Content.Cards.RegisterTrait("DisguisedPermaHint", new() {
                Icon = (_, _) => (Spr)Manifest.Sprites["IconDisguisedPermaHint"].Id!.Value,
                Name = _ => "Permanent Disguise",
                Tooltips = (_, _) => [new TTGlossary(Manifest.Glossary["DisguisedPermaHint"]?.Head!)]
            }));

            helper.ModRegistry.AwaitApi<IModSettingsApi>(
                "Nickel.ModSettings",
                settingsApi => settingsApi.RegisterModSettings(settingsApi.MakeList([
                        settingsApi.MakeCheckbox(
                                () => "Unlock all characters",
                                () => Manifest.Instance.settings.unlockAll,
                                (_, _, value) => {
                                    Manifest.Instance.settings.unlockAll = value;
                                    Manifest.Instance.settings.unlockNola = value;
                                    Manifest.Instance.settings.unlockIsa = value;
                                    Manifest.Instance.settings.unlockIlya = value;
                                    Manifest.Instance.settings.unlockJost = value;
                                    Manifest.Instance.settings.unlockGauss = value;
                                }
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK ALL CHARACTERS</c>\n" +
                            "Check this to bypass the unlock requirements for all characters in this mod. Characters can also be unlocked individually.\n" +
                            "<c=keyword>These options are reversible.</c>")
                        ]),
                        settingsApi.MakeText(() => "Unlock overrides are <c=downside>not recommended</c> for story reasons - particularly for the last two characters listed here."
                        ),
                        settingsApi.MakeCheckbox(
                                () => "   NOLA",
                                () => Manifest.Instance.settings.unlockNola,
                                (_, _, value) => Manifest.Instance.settings.unlockNola = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK NOLA</c>\n" +
                            "Bypass the unlock requirements for " + NolaColH + "Nola</c>.\n" +
                            "<c=keyword>This option is reversible.</c>")
                        ]),
                        settingsApi.MakeCheckbox(
                                () => "   ISABELLE",
                                () => Manifest.Instance.settings.unlockIsa,
                                (_, _, value) => Manifest.Instance.settings.unlockIsa = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK ISABELLE</c>\n" +
                            "Bypass the unlock requirements for " + IsaColH + "Isabelle</c>.\n" +
                            "<c=keyword>This option is reversible.</c>")
                        ]),
                        settingsApi.MakeCheckbox(
                                () => "   ILYA",
                                () => Manifest.Instance.settings.unlockIlya,
                                (_, _, value) => Manifest.Instance.settings.unlockIlya = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK ILYA</c>\n" +
                            "Bypass the unlock requirements for " + IlyaColH + "Ilya</c>.\n" +
                            "<c=keyword>This option is reversible.</c>")
                        ]),
                        settingsApi.MakeCheckbox(
                                () => "   GAUSS",
                                () => Manifest.Instance.settings.unlockGauss,
                                (_, _, value) => Manifest.Instance.settings.unlockGauss = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK GAUSS</c>\n" +
                            "Bypass the unlock requirements for " + GaussColH + "Gauss</c>.\n" +
                            "<c=keyword>This option is reversible.</c>")
                        ]),
                        settingsApi.MakeCheckbox(
                                () => "   JOST",
                                () => Manifest.Instance.settings.unlockJost,
                                (_, _, value) => Manifest.Instance.settings.unlockJost = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK JOST</c>\n" +
                            "Bypass the unlock requirements for " + JostColH + "Jost</c>.\n" +
                            "<c=keyword>This option is reversible.</c>")
                        ]),
                        settingsApi.MakeCheckbox(
                                () => "   ???",
                                () => Manifest.Instance.settings.unlockSorrel,
                                (_, _, value) => Manifest.Instance.settings.unlockSorrel = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>UNLOCK </c><c=downside>???</c>\n" +
                            "Bypass the unlock requirements for <c=downside>???</c>.\n" +
                            "<c=keyword>This option is reversible.</c>")
                        ]),
                        settingsApi.MakeCheckbox(
                                () => "??? History",
                                () => Manifest.Instance.settings.memHistory,
                                (_, _, value) => Manifest.Instance.settings.memHistory = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>LOG ???</c>\n" +
                            "These are excluded from run history by default. You can enable it here.</c>")
                        ]),
                        settingsApi.MakeEnumStepper(
                            () => "??? Difficulty",
                            () => Manifest.Instance.settings.memoryDifficulty,
                            (value) => Manifest.Instance.settings.memoryDifficulty = value
                        ).SetTooltips(() => [
                            new TTText("<c=status>??? DIFFICULTY</c>\n" +
                            "You'll know what this means when you get there.")
                        ]).SetValueWidth(
                            _ => 80
                        ).SetValueFormatter(
                            (value) => value.ToString().ToUpper()
                        ),

                    ]).SubscribeToOnMenuClose(_ => {
                        helper.Storage.SaveJson(helper.Storage.GetMainStorageFile("json"), settings);
                    })
                )
            );
        }


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
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["BulletTime"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["DefensiveFreeze"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["FrozenStun"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["Inevitability"].Id!.Value, (_, _, status) => [
                        new AHurt { targetPlayer = true, hurtAmount = 1 },
                        new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                    ]);
                    draculaApi.RegisterBloodTapOptionProvider((Status)Manifest.Statuses["CarveReality"].Id!.Value, (_, _, status) => [
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

            if (contact.LoadedManifests.Any(manifest => manifest.Name == "Shockah.Kokoro")) {
                hasKokoro = true;
                KokoroApi = contact.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
            }

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

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), "CheckDeath"),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.CheckDeathPrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), "PlayerWon"),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.PlayerWonPrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Narrative), nameof(Narrative.ActivateShoutSequence)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.NarrativeBarkPrefix))
            );

            /* draincardactions patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), nameof(Combat.DrainCardActions)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_DrainCardActions_Prefix))
            ); */

            // begincardactions patch
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Combat), "BeginCardAction"),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.Card_BeginCardAction_Prefix))
            );

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

            // character icon render prefix
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Character), nameof(Character.DrawFace)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RenderCharPrefix))
            );
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Character), nameof(Character.RenderComputer)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RenderComputerPrefix))
            );
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Character), nameof(Character.GetDisplayName),
                [typeof(string), typeof(State)]),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.CharNamePostfix))
            );
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Character), nameof(Character.RenderCharacters)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RenderCharactersPrefix))
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
                original: AccessTools.DeclaredMethod(typeof(CardReward), nameof(CardReward.TakeCard)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.TakeCardPostfix))
            );

            // unfortunately can't fix run history colors without transpiler, but this does solve some other cases.
            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Colors), nameof(Colors.LookupColor)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.LookupColorPrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(BGShowMapArt), nameof(BGShowMapArt.Render)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MapArtPostfix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(RunWinHelpers), nameof(RunWinHelpers.GetChoices)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.GetChoicesPostfix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(Dialogue), nameof(Dialogue.GetShowCockpit)),
                postfix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.GetShowCockpitPostfix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(MapBattle), nameof(MapBattle.MakeRoute)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.MakeRoutePrefix))
            );

            harmony.Patch(
                original: AccessTools.DeclaredMethod(typeof(StoryVars), nameof(StoryVars.RecordRunWin)),
                prefix: new HarmonyMethod(typeof(PatchLogic), nameof(PatchLogic.RecordRunWinPrefix))
            );


        }

        public void LoadManifest(IArtifactRegistry registry) {
            for (int i = 0; i < ManifArtifactHelper.artifactNames.Keys.Count; i++) {
                string artifact = ManifArtifactHelper.artifactNames.ElementAt(i).Key;
                if (Type.GetType("TwosCompany.Artifacts." + artifact) == null)
                    continue;
                if (NolaDeck == null || IsabelleDeck == null || IlyaDeck == null || JostDeck == null || GaussDeck == null ||
                    SorrelDeck == null)
                    throw new Exception("deck not initialized in artif registry");
                ExternalDeck pickDeck = SorrelDeck;
                if (i < 7)
                    pickDeck = NolaDeck;
                else if (i < 14)
                    pickDeck = IsabelleDeck;
                else if (i < 21)
                    pickDeck = IlyaDeck;
                else if (i < 29)
                    pickDeck = JostDeck;
                else if (i < 36)
                    pickDeck = GaussDeck;

                Artifacts.Add(artifact, new ExternalArtifact("TwosCompany.Artifacts." + artifact,
                    Type.GetType("TwosCompany.Artifacts." + artifact) ?? throw new Exception("artifact type not found: " + artifact),
                    Sprites["Icon" + artifact] ?? throw new Exception("missing MidrowProtectorProtocol sprite"),
                    ownerDeck: pickDeck));
                Artifacts[artifact].AddLocalisation(ManifArtifactHelper.artifactNames.ElementAt(i).Value.ToUpper(), 
                    ManifArtifactHelper.artifactTexts[ManifArtifactHelper.artifactNames.ElementAt(i).Key]);
                registry.RegisterArtifact(Artifacts[artifact]);
            }
        }

    }
}
