namespace GameEngine
{
    public sealed class RingRepositionedResult :
        EngineSuccessResult
    {
        public RingPosition FirstPosition { get; }
        public RingPosition SecondPosition { get; }

        internal RingRepositionedResult(
            RingPosition firstPosition,
            RingPosition secondPosition)
            : base(
                $"Repositioned {firstPosition} <-> {secondPosition}.")
        {
            FirstPosition = firstPosition;
            SecondPosition = secondPosition;
        }
    }
}