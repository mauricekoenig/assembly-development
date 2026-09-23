namespace GameEngine
{
    public sealed class ItemUsedEvent :
        GameEvent
    {
        public ItemInfo Item { get; }

        public UnitInfo Unit { get; }


        internal ItemUsedEvent(
            Item item,
            Unit unit)
        {
            Guard.NotNull(item, nameof(item));
            Guard.NotNull(unit, nameof(unit));

            Item = new ItemInfo(item);
            Unit = new UnitInfo(unit);
        }
    }
}