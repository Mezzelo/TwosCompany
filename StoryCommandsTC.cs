using System;
using TwosCompany.Artifacts;
using TwosCompany.Cards.Nola;
using TwosCompany.Cards.Isabelle;
using TwosCompany.Cards.Ilya;
using TwosCompany.Cards.Jost;
using TwosCompany.Cards.Gauss;
using TwosCompany.Cards.Sorrel;
using TwosCompany.Helper;
using TwosCompany.Zones;
using Microsoft.Extensions.Logging;


namespace TwosCompany {
    public static class StoryCommandsTC {
        public static bool MezzMemBGFriendPoof(G g) {
            if (g.state.GetDialogue()?.bg is BGCrystalizedFriend bGCrystalizedFriend) {
                bGCrystalizedFriend.DoPoof(g.state);
                foreach (String name in new string[] { "ilya", "gauss", "isa", "jost", "nola" }) {
                    if (!g.state.characters.Any((Character ch) => ch.deckType == ManifHelper.GetDeck(name))) {
                        g.state.characters.Add(new Character {
                            type = ManifHelper.GetDeck(name).Key(),
                            deckType = ManifHelper.GetDeck(name)
                        });
                        if (name.Equals("jost"))
                            g.state.SendArtifactToChar(new CrumpledWrit());

                        Ascension? artif = g.state.EnumerateAllArtifacts().OfType<Ascension>().FirstOrDefault();
                        if (artif != null) {
                            artif.characters++;
                            artif.Pulse();
                            if (artif.characters % 2 == 0)
                                g.state.ship.baseDraw--;
                        }
                        break;
                    }
                }
            }
            return true;
        }

        private static void PopulateFinaleRun(
            G g,
            HashSet<Deck> chars, 
            StarterShip shipTemplate,
            MapBase newMap,
            int difficulty,
            List<Card>? additionalCards = null,
            List<Artifact>? additionalArtifs = null,
            uint? seed = null) {
            StarterShip shipTemplate2 = shipTemplate;
            MapBase newMap2 = newMap;
            g.state.ChangeRoute(delegate {
                IEnumerable<Deck> chars2 = chars;
                g.state.SeedRand((uint)(((int?)seed) ?? Mutil.NextRandInt()));
                g.state.storyVars.ResetAfterRun();
                g.state.artifacts.Clear();
                g.state.ship = Mutil.DeepCopy(shipTemplate2.ship);
                if (difficulty >= 1) {
                    g.state.SendArtifactToChar(new HARDMODE {
                        difficulty = difficulty
                    });
                }
                g.state.deck.Clear();

                foreach (Artifact item in shipTemplate2.artifacts.Select((Artifact r) => Mutil.DeepCopy(r))) {
                    g.state.SendArtifactToChar(item);
                }

                /*
                foreach (Card item2 in shipTemplate2.cards.Select((Card card) => Mutil.DeepCopy(card))) {
                    g.state.SendCardToDeck(item2);
                } */

                g.state.characters = (from ch in chars2
                              orderby ch
                              select ch into deck
                              select new Character {
                                  type = ((deck == Deck.colorless) ? "comp" : deck.Key()),
                                  deckType = deck
                              }).ToList();
                g.state.bigStats.RecordRunStart(g.state);

                /*
                foreach (Deck item3 in chars2) {
                    if (StarterDeck.starterSets.TryGetValue(item3, out var value)) {
                        foreach (Card item4 in value.cards.Select((Card c) => c.CopyWithNewId())) {
                            g.state.SendCardToDeck(item4);
                        }
                        /* 
                        foreach (Artifact item5 in value.artifacts.Select((Artifact r) => Mutil.DeepCopy(r))) {
                            SendArtifactToChar(item5);
                        } 
                    }
                }*/

                if (additionalCards != null) {
                    foreach (Card addCard in additionalCards)
                        g.state.SendCardToDeck(addCard);
                }
                if (difficulty >= 2) {
                    g.state.SendCardToDeck(new CorruptedCore());
                }
                if (additionalArtifs != null) {
                    foreach (Artifact addArtif in additionalArtifs)
                        g.state.SendArtifactToChar(addArtif);
                }

                State state = g.state;

                state.GoToZone(newMap);
                state.ShuffleDeck();

                g.state.hideCardTooltips = false;
                return g.state.MakeZoneIntroDialogue();
            });
        }

        public static bool MezzUnlockFinaleMem(G g) {
            if (g.state.characters.Any((Character ch) => ch.deckType == ManifHelper.GetDeck("ilya")) &&
                g.state.storyVars.memoryUnlockLevel.GetValueOrDefault(ManifHelper.GetDeck("jost")) < 2) {
                g.state.storyVars.UnlockOneMemory(ManifHelper.GetDeck("jost"));
            } else if (g.state.characters.Any((Character ch) => ch.deckType == ManifHelper.GetDeck("gauss")) &&
                g.state.storyVars.memoryUnlockLevel.GetValueOrDefault(ManifHelper.GetDeck("jost")) < 3) {
                g.state.storyVars.UnlockOneMemory(ManifHelper.GetDeck("jost"));
            } else if (g.state.characters.Any((Character ch) => ch.deckType == ManifHelper.GetDeck("isa")) &&
                g.state.storyVars.memoryUnlockLevel.GetValueOrDefault(ManifHelper.GetDeck("sorrel")) < 1) {
                g.state.storyVars.UnlockOneMemory(ManifHelper.GetDeck("sorrel"));
            } else if (g.state.characters.Any((Character ch) => ch.deckType == ManifHelper.GetDeck("jost")) &&
                g.state.storyVars.memoryUnlockLevel.GetValueOrDefault(ManifHelper.GetDeck("sorrel")) < 2) {
                g.state.storyVars.UnlockOneMemory(ManifHelper.GetDeck("sorrel"));
            } else if (g.state.characters.Any((Character ch) => ch.deckType == ManifHelper.GetDeck("nola")) &&
                g.state.storyVars.memoryUnlockLevel.GetValueOrDefault(ManifHelper.GetDeck("sorrel")) < 3) {
                g.state.storyVars.UnlockOneMemory(ManifHelper.GetDeck("sorrel"));
            }
            return true;
        }

        private static Tuple<Vec, Marker> mapTuple(int x, int y, int? pathY, MapNodeContents content) {
            HashSet<int> paths = new HashSet<int>();
            if (pathY.HasValue)
                paths.Add(pathY.Value);
            return new Tuple<Vec, Marker>(
                            new Vec(x, y), new Marker() {
                                paths = paths,
                                contents = content,
                            }
                        );
        }

        public static bool NolaMem1(G g) {
            PopulateFinaleRun(
                g,
                new HashSet<Deck> {
                    ManifHelper.GetDeck("nola")
                },
                StarterShip.ships["artemis"],
                newMap: new TCFinaleMap(true) {
                    nodes = new Tuple<Vec, Marker>[] {
                        mapTuple(0, 0, 0, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(0, 1, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_1" }),
                        mapTuple(0, 2, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(0, 3, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_2" }),
                        mapTuple(0, 4, 0, new MapBattle() { battleType = BattleType.Boss, ai = new CrystalBoss() }),
                        mapTuple(0, 5, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_3" }),
                        mapTuple(0, 6, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RailCannon() }),
                        mapTuple(0, 7, null, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_4" }),
                        mapTuple(0, 10, null, new MapBattle() { battleType = BattleType.Boss, ai = new TheCobalt() }),

                        mapTuple(1, 3, 1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(1, 4, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RailCannon() }),
                        mapTuple(1, 5, 1, new MapArtifact() ),
                        mapTuple(1, 6, 1, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(1, 7, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(1, 8, 1, new MapBattle() { battleType = BattleType.Elite, ai = new CrystalMiniboss() }),
                        mapTuple(1, 9, 0, new MapBattle() { battleType = BattleType.Boss, ai = new PirateBoss() }),

                        mapTuple(8, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                        mapTuple(-9, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                    },
                    currentLocation = new Vec(1, 3),
                },
                additionalArtifs: new List<Artifact> {
                    new VestigeOfHumanity(),
                    new CommandCenter(),
                    new CrumpledWrit(),
                },
                additionalCards: new List<Card> {
                    new Onslaught(),
                    new Relentless() { upgrade = Upgrade.A },
                    new BlockShot() { upgrade = Upgrade.A },
                    new BlockParty(),
                    new Scramble() { upgrade = Upgrade.A },
                    new DrawCannon(),
                    new Lunge() { upgrade = Upgrade.A },
                    new MultiShot(),
                    new AttackDroneCard(),
                    new DroneShiftCard() { upgrade = Upgrade.A },
                    new EMPSlug(),
                    new HeatSink() { upgrade = Upgrade.A },
                    new AdminDeployCard(),
                    new RerollCard() { upgrade = Upgrade.A },
                    new UnpoweredShardCard(),
                    new MageHand() { upgrade = Upgrade.A },
                    new Flourish(),
                    new Sideswipe() { upgrade = Upgrade.A },
                    new Galvanize() { upgrade = Upgrade.A },
                    new MoltenShot(),
                    new FrontGuard(),
                    new OverheadBlow() { upgrade = Upgrade.A },
                    new ConduitCard(),
                    new SparkCard() { upgrade = Upgrade.A },
                },
                difficulty: (int) Manifest.Instance.settings.memoryDifficulty
            );
            return true;
        }

        public static bool IsaMem1(G g) {
            PopulateFinaleRun(
                g,
                new HashSet<Deck> {
                    ManifHelper.GetDeck("isa")
                },
                StarterShip.ships["artemis"],
                newMap: new TCFinaleMap(false) {
                    nodes = new Tuple<Vec, Marker>[] {
                        mapTuple(0, 0, 0, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(0, 1, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_1" }),
                        mapTuple(0, 2, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(0, 3, null, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_2" }),
                        mapTuple(0, 10, null, new MapBattle() { battleType = BattleType.Boss, ai = new TheCobalt() }),

                        mapTuple(1, 3, 1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(1, 4, 1, new MapBattle() { battleType = BattleType.Normal, ai = new RailCannon() }),
                        mapTuple(1, 5, 1, new MapArtifact() ),
                        mapTuple(1, 6, 1, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(1, 7, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(1, 8, 1, new MapShop()),
                        mapTuple(1, 9, 0, new MapBattle() { battleType = BattleType.Elite, ai = new PirateBoss() }),

                        mapTuple(2, 2, 2, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(2, 3, 2, new MapBattle() { battleType = BattleType.Normal, ai = new MediumFighterZone3() }),
                        mapTuple(2, 4, 2, new MapBattle() { battleType = BattleType.Elite, ai = new StoneGuy() }),
                        mapTuple(2, 5, 2, new MapArtifact() ),
                        mapTuple(2, 6, 2, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(2, 7, 2, new MapShop()),
                        mapTuple(2, 8, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),

                        mapTuple(-1, 3, -1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(-1, 4, -1, new MapBattle() { battleType = BattleType.Elite, ai = new RailCannon() }),
                        mapTuple(-1, 5, -1, new MapArtifact() ),
                        mapTuple(-1, 6, -1, new MapBattle() { battleType = BattleType.Boss, ai = new CrystalBoss() }),
                        mapTuple(-1, 7, -1, new MapBattle() { battleType = BattleType.Normal, ai = new MissileCowardZ3() }),
                        mapTuple(-1, 8, -1, new MapBattle() { battleType = BattleType.Elite, ai = new CrystalMiniboss() }),
                        mapTuple(-1, 9, 0, new MapBattle() { battleType = BattleType.Elite, ai = new AsteroidDriller() }),

                        mapTuple(8, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                        mapTuple(-9, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                    },
                    currentLocation = new Vec(-1, 3),
                },
                additionalArtifs: new List<Artifact> {
                    new LongLostRegrets(),
                    new Metronome()
                },
                additionalCards: new List<Card> {
                    new Flourish(),
                    new Sideswipe(),
                    new Misdirection(),
                    new Bind() { upgrade = Upgrade.B },
                    new LightningStrikes() { upgrade = Upgrade.B },
                    new Couch() { upgrade = Upgrade.B },
                    new Remise(),
                    new Riposte() { upgrade = Upgrade.A },
                    new CannonColorless(),
                    new DodgeColorless(),
                    new BasicShieldColorless()
                },
                difficulty: (int)Manifest.Instance.settings.memoryDifficulty
            );
            return true;
        }

        public static bool IlyaMem1(G g) {
            PopulateFinaleRun(
                g,
                new HashSet<Deck> {
                    ManifHelper.GetDeck("ilya")
                },
                StarterShip.ships["artemis"],
                newMap: new TCFinaleMap(false) {
                    nodes = new Tuple<Vec, Marker>[] {
                        mapTuple(0, 10, null, new MapBattle() { battleType = BattleType.Boss, ai = new TheCobalt() }),

                        mapTuple(1, 3, 1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(1, 4, 1, new MapBattle() { battleType = BattleType.Normal, ai = new RailCannon() }),
                        mapTuple(1, 5, 1, new MapArtifact() ),
                        mapTuple(1, 6, 1, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(1, 7, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(1, 8, 1, new MapShop()),
                        mapTuple(1, 9, 0, new MapBattle() { battleType = BattleType.Normal, ai = new PirateBoss() }),

                        mapTuple(2, 2, 2, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(2, 3, 2, new MapBattle() { battleType = BattleType.Normal, ai = new MediumFighterZone3() }),
                        mapTuple(2, 4, 2, new MapBattle() { battleType = BattleType.Elite, ai = new StoneGuy() }),
                        mapTuple(2, 5, 2, new MapArtifact() ),
                        mapTuple(2, 6, 2, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(2, 7, 2, new MapShop()),
                        mapTuple(2, 8, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        
                        mapTuple(-1, 3, -1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(-1, 4, -1, new MapBattle() { battleType = BattleType.Normal, ai = new RailCannon() }),
                        mapTuple(-1, 5, -1, new MapArtifact() ),
                        mapTuple(-1, 6, -1, new MapBattle() { battleType = BattleType.Boss, ai = new CrystalBoss() }),
                        mapTuple(-1, 7, -1, new MapBattle() { battleType = BattleType.Normal, ai = new MissileCowardZ3() }),
                        mapTuple(-1, 8, -1, new MapShop()),
                        mapTuple(-1, 9, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),

                        mapTuple(-2, 3, -2, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(-2, 4, -2, new MapBattle() { battleType = BattleType.Normal, ai = new RailCannon() }),
                        mapTuple(-2, 5, -2, new MapArtifact() ),
                        mapTuple(-2, 6, -2, new MapBattle() { battleType = BattleType.Boss, ai = new PirateBoss() }),
                        mapTuple(-2, 7, -2, new MapBattle() { battleType = BattleType.Elite, ai = new AsteroidDriller() }),
                        mapTuple(-2, 8, -1, new MapShop() ),

                        mapTuple(-3, 2, -3, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(-3, 3, -3, new MapBattle() { battleType = BattleType.Normal, ai = new MediumAncient() }),
                        mapTuple(-3, 4, -3, new MapBattle() { battleType = BattleType.Elite, ai = new AsteroidDriller() }),
                        mapTuple(-3, 5, -3, new MapArtifact() ),
                        mapTuple(-3, 6, -3, new MapBattle() { battleType = BattleType.Boss, ai = new PirateBoss() }),
                        mapTuple(-3, 7, -2, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),

                        mapTuple(8, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                        mapTuple(-9, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                    },
                    currentLocation = new Vec(-3, 2),
                },
                additionalArtifs: new List<Artifact> {
                    new EternalFlame(),
                    new AncientMatchbox(),
                    new IncendiaryRounds(),
                },
                additionalCards: new List<Card> {
                    new Apex() { upgrade = Upgrade.B },
                    new MoltenShot(),
                    new Haze(),
                    new Ignition() { upgrade = Upgrade.A },
                    new Immolate() { upgrade = Upgrade.B },
                    new ThermalBlast()  { upgrade = Upgrade.A },
                    new HeatTreatment() { upgrade = Upgrade.A },
                    new DragonsBreath() { upgrade = Upgrade.B },
                    new Burnout(),
                    new BlastShield(),
                    new DodgeColorless(),
                    new BasicShieldColorless()
                },
                difficulty: (int) Manifest.Instance.settings.memoryDifficulty
            );
            return true;
        }


        public static bool JostMem1(G g) {
            PopulateFinaleRun(
                g,
                new HashSet<Deck> {
                    ManifHelper.GetDeck("jost")
                },
                StarterShip.ships["artemis"],
                newMap: new TCFinaleMap(true) {
                    nodes = new Tuple<Vec, Marker>[] {
                        mapTuple(0, 0, 0, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(0, 1, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_1" }),
                        mapTuple(0, 2, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(0, 3, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_2" }),
                        mapTuple(0, 4, 0, new MapBattle() { battleType = BattleType.Boss, ai = new CrystalBoss() }),
                        mapTuple(0, 5, null, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_3" }),
                        mapTuple(0, 10, null, new MapBattle() { battleType = BattleType.Boss, ai = new TheCobalt() }),

                        mapTuple(1, 3, 1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(1, 4, 1, new MapBattle() { battleType = BattleType.Elite, ai = new StoneGuy() }),
                        mapTuple(1, 5, 1, new MapArtifact() ),
                        mapTuple(1, 6, 1, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(1, 7, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(1, 8, 1, new MapBattle() { battleType = BattleType.Elite, ai = new AsteroidDriller() }),
                        mapTuple(1, 9, 0, new MapBattle() { battleType = BattleType.Boss, ai = new PirateBoss() }),

                        mapTuple(2, 2, 2, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(2, 3, 2, new MapBattle() { battleType = BattleType.Normal, ai = new MediumFighterZone3() }),
                        mapTuple(2, 4, 2, new MapBattle() { battleType = BattleType.Elite, ai = new StoneGuy() }),
                        mapTuple(2, 5, 2, new MapArtifact() ),
                        mapTuple(2, 6, 2, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(2, 7, 2, new MapBattle() { battleType = BattleType.Elite, ai = new AsteroidDriller() }),
                        mapTuple(2, 8, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),

                        mapTuple(8, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                        mapTuple(-9, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                    },
                    currentLocation = new Vec(2, 2),
                },
                additionalArtifs: new List<Artifact> {
                    new AimlessVengeance(),
                    new MilitiaArmband(),
                    new AbandonedTassels()
                },
                additionalCards: new List<Card> {
                    new FrontGuard(),
                    new HighGuard() { upgrade = Upgrade.A },
                    new Limitless() { upgrade = Upgrade.B },
                    new OverheadBlow() { upgrade = Upgrade.B },
                    new SweepingStrikes() { upgrade = Upgrade.A },
                    new StandFirm(),
                    new ReactiveDefense(),
                    new RecklessAbandon(),
                    new Retribution() { upgrade = Upgrade.A },
                    new CannonColorless(),
                    new DodgeColorless(),
                    new BasicShieldColorless()
                },
                difficulty: (int) Manifest.Instance.settings.memoryDifficulty
            );
            return true;
        }

        public static bool GaussMem1(G g) {
            PopulateFinaleRun(
                g,
                new HashSet<Deck> {
                    ManifHelper.GetDeck("gauss")
                },
                StarterShip.ships["artemis"],
                newMap: new TCFinaleMap(false) {
                    nodes = new Tuple<Vec, Marker>[] {
                        mapTuple(0, 0, 0, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(0, 1, null, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_1" }),
                        mapTuple(0, 10, null, new MapBattle() { battleType = BattleType.Boss, ai = new TheCobalt() }),

                        mapTuple(1, 3, 1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(1, 4, 1, new MapBattle() { battleType = BattleType.Normal, ai = new RailCannon() }),
                        mapTuple(1, 5, 1, new MapArtifact() ),
                        mapTuple(1, 6, 1, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(1, 7, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(1, 8, 1, new MapShop()),
                        mapTuple(1, 9, 0, new MapBattle() { battleType = BattleType.Normal, ai = new PirateBoss() }),

                        mapTuple(2, 2, 2, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(2, 3, 2, new MapBattle() { battleType = BattleType.Normal, ai = new MediumFighterZone3() }),
                        mapTuple(2, 4, 2, new MapBattle() { battleType = BattleType.Elite, ai = new StoneGuy() }),
                        mapTuple(2, 5, 2, new MapArtifact() ),
                        mapTuple(2, 6, 2, new MapBattle() { battleType = BattleType.Boss, ai = new Z1BossFreeze() }),
                        mapTuple(2, 7, 2, new MapShop()),
                        mapTuple(2, 8, 1, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),

                        mapTuple(-1, 3, -1, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(-1, 4, -1, new MapBattle() { battleType = BattleType.Elite, ai = new RailCannon() }),
                        mapTuple(-1, 5, -1, new MapArtifact() ),
                        mapTuple(-1, 6, -1, new MapBattle() { battleType = BattleType.Boss, ai = new CrystalBoss() }),
                        mapTuple(-1, 7, -1, new MapBattle() { battleType = BattleType.Normal, ai = new MissileCowardZ3() }),
                        mapTuple(-1, 8, -1, new MapShop()),
                        mapTuple(-1, 9, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),

                        mapTuple(-2, 3, -2, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(-2, 4, -2, new MapBattle() { battleType = BattleType.Elite, ai = new AsteroidBoss() }),
                        mapTuple(-2, 5, -2, new MapArtifact() ),
                        mapTuple(-2, 6, -2, new MapBattle() { battleType = BattleType.Boss, ai = new Z2BossMechaPossum() }),
                        mapTuple(-2, 7, -2, new MapBattle() { battleType = BattleType.Elite, ai = new RailCannon() }),
                        mapTuple(-2, 8, -1, new MapShop()),

                        mapTuple(8, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                        mapTuple(-9, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                    },
                    currentLocation = new Vec(-2, 3),
                },
                additionalArtifs: new List<Artifact> {
                    new TearItAllDown(),
                    new ExoticMetals()
                },
                additionalCards: new List<Card> {
                    new ConduitCard() { upgrade = Upgrade.B },
                    new SparkCard()  { upgrade = Upgrade.A },
                    new ConductorArray(),
                    new SolderShuffle(),
                    new StrikeTwice(),
                    new ShieldConduit(),
                    new CannonColorless(),
                    new DodgeColorless(),
                    new BasicShieldColorless()
                },
                difficulty: (int)Manifest.Instance.settings.memoryDifficulty
            );
            return true;
        }

        public static bool SorrelMem1(G g) {
            PopulateFinaleRun(
                g,
                new HashSet<Deck> {
                    ManifHelper.GetDeck("sorrel")
                },
                StarterShip.ships["artemis"],
                newMap: new TCFinaleMap(true) {
                    nodes = new Tuple<Vec, Marker>[] {
                        mapTuple(0, 0, 0, new MapBattle() { battleType = BattleType.Normal, ai = new LightFighterZone3() }),
                        mapTuple(0, 1, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_1" }),
                        mapTuple(0, 2, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RustingColossus() }),
                        mapTuple(0, 3, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_2" }),
                        mapTuple(0, 4, 0, new MapBattle() { battleType = BattleType.Boss, ai = new CrystalBoss() }),
                        mapTuple(0, 5, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_3" }),
                        mapTuple(0, 6, 0, new MapBattle() { battleType = BattleType.Elite, ai = new RailCannon() }),
                        mapTuple(0, 7, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_4" }),
                        mapTuple(0, 8, 0, new MapBattle() { battleType = BattleType.Elite, ai = new CrystalMiniboss() }),
                        mapTuple(0, 9, 0, new TCFinaleEvent() { story = "mezz_Sorrel_TCFinaleEvent_5" }),
                        mapTuple(0, 10, null, new MapBattle() { battleType = BattleType.Boss, ai = new TheCobalt() }),

                        mapTuple(8, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                        mapTuple(-9, 7, null, new MapBattle() { battleType = BattleType.Normal, ai = new UnderwaterGuy() }),
                    },
                    currentLocation = new Vec(0, 0),
                },
                additionalArtifs: new List<Artifact> {
                    new Ascension(),
                },
                additionalCards: new List<Card> {
                    new DeusExMachina(),
                    new Postpone(),
                    new FaceDownFate(),
                    new MorningDew(),
                    new Collapse() { upgrade = Upgrade.A },
                    new WaveOfTheHand() { upgrade = Upgrade.A },
                    new DodgeColorless(),
                    new BasicShieldColorless()
                },
                difficulty: (int)Manifest.Instance.settings.memoryDifficulty
            );
            return true;
        }
    }
}
