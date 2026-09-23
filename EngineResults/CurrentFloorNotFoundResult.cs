namespace GameEngine
{
    public sealed class CurrentFloorNotFoundResult : EngineFailedResult
    {
        public CurrentFloorNotFoundResult()
            : base("The current floor could not be found.")
        {
        }
    }
}