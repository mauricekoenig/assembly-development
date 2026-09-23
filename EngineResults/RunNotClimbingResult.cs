namespace GameEngine
{
    public sealed class RunNotClimbingResult : EngineFailedResult
    {
        public RunNotClimbingResult()
            : base("The run is not currently climbing.")
        {
        }
    }
}