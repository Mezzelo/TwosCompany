using CobaltCoreModding.Definitions.ExternalItems;

namespace TwosCompany {
    public interface ITwosAPI {
        ExternalDeck NolaDeck { get; }
        ExternalDeck IsabelleDeck { get; }
        ExternalDeck IlyaDeck { get; }
        ExternalDeck JostDeck { get; }
        ExternalDeck GaussDeck { get; }
    }
}
