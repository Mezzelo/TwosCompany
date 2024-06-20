namespace TwosCompany.Actions {
    using System.Collections.Generic;
    using static System.Net.Mime.MediaTypeNames;
    using TwosCompany.Artifacts;
    using TwosCompany.Helper;
    using TwosCompany.Midrow;

    public class AMoveEnemy : AMove {
        public override List<Tooltip> GetTooltips(State s) => new List<Tooltip>() {
            new TTGlossary(Manifest.Glossary[
                "MoveEnemy" + (dir > 0 ? "Right" : dir < 0 ? "Left" : "Zero")
                ]?.Head ?? throw new Exception("missing glossary entry: moveenemy"), Math.Abs(dir)),
            };
    }
}
