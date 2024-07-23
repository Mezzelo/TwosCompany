using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany {

    public sealed class APIImplementation : ITwosAPI {
        public ExternalDeck NolaDeck => Manifest.NolaDeck!;
        public ExternalDeck IsabelleDeck => Manifest.IsabelleDeck!;
        public ExternalDeck IlyaDeck => Manifest.IlyaDeck!;
        public ExternalDeck JostDeck => Manifest.JostDeck!;
        public ExternalDeck GaussDeck => Manifest.GaussDeck!;
        public ExternalDeck SorrelDeck => Manifest.SorrelDeck!;
    }
}