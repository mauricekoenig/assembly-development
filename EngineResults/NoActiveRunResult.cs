namespace GameEngine
{
    public sealed class NoActiveRunResult : EngineFailedResult
    {
        public NoActiveRunResult()
            : base("There is no active run.")
        {
        }
    }
}