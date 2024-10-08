namespace TwosCompany.Cards.Nola {
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class DoubleDown : Card {
        public override CardData GetData(State state) {
            string cardText;
            if (upgrade == Upgrade.None)
                cardText = Loc.GetLocString(Manifest.Cards?["DoubleDown"].DescLocKey ?? throw new Exception("Missing card description"));
            else if (upgrade == Upgrade.A)
                cardText = Loc.GetLocString(Manifest.Cards?["DoubleDown"].DescALocKey ?? throw new Exception("Missing card description"));
            else
                cardText = Loc.GetLocString(Manifest.Cards?["DoubleDown"].DescBLocKey ?? throw new Exception("Missing card description"));

            return new CardData() {
                cost = upgrade == Upgrade.B ? 3 : 2,
                description = cardText,
                retain = upgrade == Upgrade.A,
                exhaust = true,
            };
        }

        public override List<CardAction> GetActions(State s, Combat c) {
            List<CardAction> actions = new List<CardAction>();

            Ship ship = s.ship;
            foreach (KeyValuePair<Status, int> thisStatus in ship.statusEffects) {
                if (thisStatus.Value == 0)
                    continue;
                if (
                    thisStatus.Key != Status.shield
                    && thisStatus.Key != Status.tempShield
                    // && thisStatus.Key != Status.maxShield
                    // && thisStatus.Key != Status.shard
                    // && thisStatus.Key != Status.evade
                    // && thisStatus.Key != Status.maxShard
                    )
                    actions.Add(new AStatus() {
                        targetPlayer = true,
                        status = thisStatus.Key,
                        statusAmount = upgrade == Upgrade.B ? thisStatus.Value : 1
                    });
            }
            return actions;
        }

        public override string Name() => "Double Down";
    }
}
