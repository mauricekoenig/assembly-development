namespace GameEngine
{
    public sealed class NoActiveEventActivityResult : EngineFailedResult
    {
        public NoActiveEventActivityResult()
            : base("The active event is not waiting for an activity result.")
        {
        }
    }
}
