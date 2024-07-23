
namespace TwosCompany.Zones {
    public class TCFinaleMap : MapBase {

        public bool rays = false;
        public TCFinaleMap(bool rays) {
            this.rays = rays;
        }

        public override Color GetPrimaryColor() {
            return new Color("151144");
        }

        public override Color GetVanillaGlowColor() {
            return new Color("151144").gain(0.4);
        }

        public override Color GetMapLineColor() {
            return new Color("092a86");
        }

        public override Color GetUnvisitedColor() {
            return GetMapLineColor().gain(1.2);
        }

        public override Song GetSongIdForMap(State state) {
            return Song.Three;
        }

        public override string GetZoneDialogueTag() {
            return "zone_three";
        }

        public override string GetVictoryDialogue() {
            return ".mezzTCCloser";
        }

        public override bool IsFinalZone() {
            return false;
        }

        public override double GetUpgradeChance() {
            return 1.0;
        }

        public override MapSpawnConfig GetSpawnConfig(State s, Rand rng) {
            return new MapSpawnConfig {
                elites = 3,
                misc = new Stack<MapNodeContents>(),
                shops = 0,
                events = 0
            };
        }

        public Tuple<Vec, Marker>[] nodes = new Tuple<Vec, Marker>[0];

        public override void Populate(State s, Rand rng) {
            foreach (Tuple<Vec, Marker> node in nodes) {
                markers.Add(node.Item1, node.Item2);
            }
            /*
            int y = 0;
            int lastY = 0;
            for (int i = 0; i < 11; i++) {
                if (i > 0)
                    y = Math.Clamp(y + rng.NextInt() % 3 - 1, -3, 3);
                markers.Add(new Vec(y, i), new Marker() {
                    contents = 
                    i == 3 ? new MapArtifact() : 
                    i == 2 || i == 5 || i == 6 ? new MapBattle() {
                        battleType = BattleType.Elite,
                    } :
                    i == 10 ? new MapBattle() {
                        battleType = BattleType.Boss,
                    } :
                    new MapBattle() {
                        battleType = BattleType.Normal,
                    },
                });
                if (i > 0)
                    markers[new Vec(lastY, i - 1)].paths.Add(y);
                lastY = y;
            }*/

        }

        public override MapEnemyPool GetEnemyPools(State s) {
            MapEnemyPool mapEnemyPool = new MapEnemyPool {
                easy = { new LightFighterZone2() },
                normal =
                {
                new HeavyFighter(),
                new LightScouter(),
                new DualDroneBaddie()
            },
                elites =
                {
                new WideCruiser(),
                new DrakePirate(),
                new PaybackBruiserZ1()
            },
                bosses = { 
                    new CrystalBoss(),
                }
            };

            return mapEnemyPool;
        }
    }
}