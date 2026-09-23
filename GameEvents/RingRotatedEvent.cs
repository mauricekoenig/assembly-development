namespace GameEngine
{
    public sealed class RingRotatedEvent : GameEvent
    {
        public RingInfo Ring { get; }

        internal RingRotatedEvent(RingInfo ring)
        {
            Guard.NotNull(ring, nameof(ring));

            Ring = ring;
        }
    }
}