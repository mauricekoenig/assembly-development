namespace GameEngine
{
    public sealed class NoActiveEventPresentationResult
        : EngineFailedResult
    {
        public NoActiveEventPresentationResult()
            : base(
                "The active event is not waiting on a Presentation node.")
        {
        }
    }
}