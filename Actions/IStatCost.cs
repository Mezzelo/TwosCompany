namespace TwosCompany.Actions {
    interface IStatCost {
        Status statusReq { get; }
        int statusCost { get; }
        int cumulative { get; }
        CardAction? action  { get; }
        bool first { get; }
}
}
