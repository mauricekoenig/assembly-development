namespace GameEngine
{
    public sealed class UnitAddedEvent : GameEvent
    {
        public UnitInfo Unit { get; }

        internal UnitAddedEvent(UnitInfo unit)
        {
            Guard.NotNull(unit, nameof(unit));

            Unit = unit;
        }
    }
}