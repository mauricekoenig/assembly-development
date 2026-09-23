namespace GameEngine
{
    public sealed class RingRotatedResult : EngineSuccessResult
    {
        public RotationDirection Direction { get; }

        public RingRotatedResult(RotationDirection direction)
            : base($"The ring was rotated {direction}.")
        {
            Direction = direction;
        }
    }
}