namespace GameEngine
{
    public sealed class EventOptionUnavailableResult : EngineFailedResult
    {
        public EventOptionUnavailableResult(string reason)
            : base(string.IsNullOrWhiteSpace(reason)
                ? "The selected event option is not available."
                : reason)
        {
        }
    }
}
