namespace GameEngine
{
    public sealed class TeamFullResult : EngineFailedResult
    {
        public TeamFullResult()
            : base("The team is full.")
        {
        }
    }
}