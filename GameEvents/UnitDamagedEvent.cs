namespace GameEngine
{
    public sealed class UnitDamagedEvent : GameEvent
    {
        public UnitInfo Unit { get; }
        public int Amount { get; }

        internal UnitDamagedEvent(
            UnitInfo unit,
            int amount)
        {
            Guard.NotNull(unit, nameof(unit));

            Unit = unit;
            Amount = amount;
        }
    }
}