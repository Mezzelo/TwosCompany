namespace TwosCompany.Actions {
    public class ATurnIncreaseHint : CardAction {
        public int amount = 1;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconTurnIncreaseCost"].Id ?? throw new Exception("missing icon")), amount, Colors.downside);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["TurnIncreaseCost"]?.Head ??
            throw new Exception("missing glossary entry: TurnIncreaseCost"), amount) };
    }
}