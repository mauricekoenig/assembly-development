namespace GameEngine
{
    public sealed class RotationFailedResult : EngineFailedResult
    {
        public RotationFailedResult()
            : base("The ring could not be rotated.")
        {
        }
    }
}