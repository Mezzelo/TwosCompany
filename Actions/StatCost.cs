namespace TwosCompany.Actions {
    interface StatCost {
        Status statusReq { get; }
        int statusCost { get; }
        int cumulative { get; }
        CardAction? action  { get; }
        bool first { get; }
}
}
