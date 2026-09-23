namespace GameEngine
{
    public sealed class UnitDiedEvent : GameEvent
    {
        public UnitInfo Unit { get; }

        internal UnitDiedEvent(UnitInfo unit)
        {
            Guard.NotNull(unit, nameof(unit));

            Unit = unit;
        }
    }
}