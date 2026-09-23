namespace GameEngine
{
    public sealed class ClimbRequirementsNotMetResult : EngineFailedResult
    {
        public ClimbRequirementsNotMetResult()
            : base("The requirements to begin the climb are not met.")
        {
        }
    }
}