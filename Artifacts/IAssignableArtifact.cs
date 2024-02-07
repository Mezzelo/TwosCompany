namespace TwosCompany.Artifacts {
    interface IAssignableArtifact {
        int assignedUUID { set; }
        TTCard cardImpression { set; }
        // Card disguisedAs { get; }
    }
}
