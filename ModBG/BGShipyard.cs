namespace TwosCompany.ModBG {
    public class BGShipyard : BG {
        public bool silence = false;
        public override void Render(G g, double t, Vec offset) {
            _ = g.state.map;
            offset = default(Vec);
            Color color = new Color(0.05, 0.2, 0.15).gain(0.8);
            Draw.Fill(color);
            BGComponents.NormalStars(g, t, offset);
            BGComponents.RegularGlowMono(g, offset, color);
            Spr? id = Enum.Parse<Spr>("bg_space_station_alt");
            Color? color2 = color;
            Vec? originRel = new Vec(0.5, 0.5);
            Draw.Sprite(id, -70.0, 150.0, flipX: true, flipY: false, 0.0, null, originRel, null, null, color2);
            BGComponents.RegularGlowMono(g, new Vec(0.0, g.state.map.age * -100.0), new Color(0.0, 0.7, 0.5).gain(0.5));
            Spr? id6 = Enum.Parse<Spr>("bg_space_station_alt");
            color2 = color;
            originRel = new Vec(0.5, 0.5);
            Draw.Sprite(id6, 400.0, 0.0, flipX: false, flipY: false, 0.0, null, originRel, null, null, color2);
            Spr? id2 = Enum.Parse<Spr>("bg_space_station_alt");
            originRel = new Vec(0.0, 0.0);
            Draw.Sprite(id2, -50.0, 150.0, flipX: false, flipY: false, 0.0, null, originRel, null, null, new Color(0.0, 0.0, 0.0));
            Spr? id5 = Enum.Parse<Spr>("bg_shipyard_platform");
            Vec? originRel2 = new Vec(0.0, 1.0);
            Draw.Sprite(id5, 350.0, 200.0, flipX: false, flipY: false, 0.0, null, originRel2);
        }
        public override void OnAction(State s, string action) {
            if (action == "silence_cue")
                silence = true;
        }
        public MusicState GetMusicState() => new MusicState() {
            scene = silence ? Song.SlowSilence : Song.Vault
        };
    }
}