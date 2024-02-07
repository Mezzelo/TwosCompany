namespace TwosCompany.Actions {
    public class ACostIncreasePlayedHint : CardAction {
        public int amount = 1;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconEnergyPerPlay"].Id ?? throw new Exception("missing icon")), amount, Colors.downside);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["EnergyPerPlay"]?.Head ?? 
            throw new Exception("missing glossary entry: EnergyPerPlay"), amount) };
    }
    public class ACostDecreasePlayedHint : CardAction {
        public int amount = 1;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconLowerPerPlay"].Id ?? throw new Exception("missing icon")), amount, Colors.textMain);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["LowerPerPlay"]?.Head
            ?? throw new Exception("missing glossary entry: LowerPerPlay"), amount) };

    }
    public class ACostDecreaseAttackHint : CardAction {
        public int amount = 1;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconEnergyPerAttack"].Id ?? throw new Exception("missing icon")), amount, Colors.textMain);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["EnergyPerAttack"]?.Head ??
            throw new Exception("missing glossary entry: EnergyPerAttack"), amount) };
    }
    public class AOtherPlayedHint : CardAction {
        public int amount = 1;
        public bool perma = false;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconEnergyPerCard" + (perma ? "Perma" : "")].Id ?? 
            throw new Exception("missing icon")), amount, Colors.downside);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["EnergyPerCard" + (perma ? "Perma" : "")]?.Head ??
            throw new Exception("missing glossary entry: EnergyPerCard"), amount) };
    }
    public class ATurnIncreaseHint : CardAction {
        public int amount = 1;
        public override void Begin(G g, State s, Combat c) => this.timer = 0.0;

        public override Icon? GetIcon(State s) => new Icon((Spr)(Manifest.Sprites["IconTurnIncreaseCost"].Id ?? throw new Exception("missing icon")), amount, Colors.downside);

        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() { new TTGlossary(Manifest.Glossary["TurnIncreaseCost"]?.Head ??
            throw new Exception("missing glossary entry: TurnIncreaseCost"), amount) };
    }
}