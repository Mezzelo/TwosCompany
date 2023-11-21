using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.Cards.Nola
{
    [CardMeta(rarity = Rarity.common, dontOffer = true)]
    public class FlexibleDodge : Card
    {
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.Add(new ADrawCard()
                    {
                        count = 2
                    });
                    break;
                case Upgrade.A:
                    result.Add(new ADrawCard()
                    {
                        count = 1
                    }
                    );
                    break;
                case Upgrade.B:
                    result.Add(new ADrawCard()
                    {
                        count = 4
                    }
                    );
                    break;
            }
            return result;
        }

        public override CardData GetData(State state)
        {
            return new CardData
            {
                cost = upgrade == Upgrade.A ? 0 : 1
            };
        }

        public override string Name() => "Flexible Dodge";
    }
}