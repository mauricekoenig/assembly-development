namespace GameEngine
{
    public sealed class RingRepositionedEvent : GameEvent
    {
        public RingPosition FirstPosition { get; }
        public RingPosition SecondPosition { get; }

        public RingInfo Ring { get; }

        internal RingRepositionedEvent(
            RingPosition firstPosition,
            RingPosition secondPosition,
            RingInfo ring)
        {
            Guard.NotNull(ring, nameof(ring));

            FirstPosition = firstPosition;
            SecondPosition = secondPosition;
            Ring = ring;
        }
    }
}