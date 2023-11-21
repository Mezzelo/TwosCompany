namespace TwosCompany.Actions {
    public class AAllIncreaseHint : CardAction {
        public int amount = 1;
        public bool isCombat = false;

        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites[isCombat ? "IconAllIncreaseCombat" : "IconAllIncrease"].Id ?? 
            throw new Exception("missing icon")), amount, Colors.status);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary[isCombat ? "AllIncreaseCombat" : "AllIncrease"]?.Head ??
            throw new Exception("missing glossary entry: AllIncrease"), amount) };
    }
}