namespace TwosCompany.Artifacts {
    interface IOnMoveArtifact {
        void Movement(int dist, bool targetPlayer, bool fromEvade, Combat c, State s);
    }
}
