namespace GameEngine
{
    public sealed class EventActivityResultNotFoundResult : EngineFailedResult
    {
        public EventActivityResultNotFoundResult(string resultId)
            : base($"Event activity result '{resultId}' could not be found.")
        {
        }
    }
}
