namespace TwosCompany.ModBG {
    public class BGBlackSilent : BG {

        public override void Render(G g, double t, Vec offset) {
            Draw.Fill(Colors.black);
        }
    }
}
