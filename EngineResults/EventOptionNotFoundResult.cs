namespace GameEngine
{
    public sealed class EventOptionNotFoundResult
        : EngineFailedResult
    {
        public EventOptionNotFoundResult()
            : base("The selected event option could not be found.")
        {
        }
    }
}