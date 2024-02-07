using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany.Cards {
    public static class Stance {
        public static int Get(State s) {
            int stance = 1;
            if (s.route is Combat c) {
                ExternalStatus defensiveStance = Manifest.Statuses?["DefensiveStance"] ?? throw new Exception("status missing: defensivestance");
                ExternalStatus offensiveStance = Manifest.Statuses?["OffensiveStance"] ?? throw new Exception("status missing: offensivestance");
                stance = (s.ship.Get((Status)defensiveStance.Id!) > 0 ? 1 : 0) + (s.ship.Get((Status)offensiveStance.Id!) > 0 ? 2 : 0);
            }
            return stance;
        }

        public static String AppendName(State s) {
            return Stance.Get(s) switch { 0 => "Neither", 2 => "Flip", 3 => "Both", _ => "" };
        }
    }
}
