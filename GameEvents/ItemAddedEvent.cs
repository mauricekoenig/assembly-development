namespace GameEngine
{
    public sealed class ItemAddedEvent :
        GameEvent
    {
        public ItemInfo Item { get; }


        internal ItemAddedEvent(
            ItemInfo item)
        {
            Guard.NotNull(item, nameof(item));

            Item = item;
        }
    }
}