namespace TwosCompany.ModBG {
    public class BGRunStartScripted : BG {
        public bool silence;

        public override void OnAction(State s, string action) {
            if (action == "silence_cue")
                silence = true;
        }
        public override void Render(G g, double t, Vec offset) {
            Color color = new Color(0.0, 0.1, 0.2).gain(0.5);
            Draw.Fill(color);
            BGComponents.NormalStars(g, t, offset);
            BGComponents.RegularNebula(g, offset, color);
        }
        public MusicState GetMusicState() => new MusicState() {
            scene = silence ? Song.SlowSilence : Song.Epoch };
    }
}
