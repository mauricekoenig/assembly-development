namespace GameEngine
{
    public sealed class SectorActivationStartedEvent : GameEvent
    {
        public RingPosition Position { get; }

        public SectorType SectorType { get; }


        internal SectorActivationStartedEvent(
            RingPosition position,
            SectorType sectorType)
        {
            Position =
                position;

            SectorType =
                sectorType;
        }
    }
}
