namespace TwosCompany.ExternalAPI {
    public interface IDraculaApi {
        void RegisterBloodTapOptionProvider(Status status, Func<State, Combat, Status, List<CardAction>> provider);
    }
}
