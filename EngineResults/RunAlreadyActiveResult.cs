namespace GameEngine
{
    public sealed class RunAlreadyActiveResult : EngineFailedResult
    {
        public RunAlreadyActiveResult()
            : base("A run is already active.")
        {
        }
    }
}