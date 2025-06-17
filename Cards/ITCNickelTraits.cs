using Nickel.Legacy;
using Nickel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwosCompany.Cards {
    internal interface ITCNickelTraits : IHasCustomCardTraits {
        string[] GetTraits()
            => new string[] { };

        IReadOnlySet<ICardTraitEntry> IHasCustomCardTraits.GetInnateTraits(State state) {
            var traits = new HashSet<ICardTraitEntry>();
            foreach (String trait in GetTraits()) {
                traits.Add(Manifest.Traits[trait]);
            }
            return traits;
        }
    }
}
