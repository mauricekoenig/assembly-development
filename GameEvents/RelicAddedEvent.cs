namespace GameEngine
{
    public sealed class RelicAddedEvent :
        GameEvent
    {
        public RelicInfo Relic { get; }


        internal RelicAddedEvent(
            RelicInfo relic)
        {
            Guard.NotNull(relic, nameof(relic));

            Relic = relic;
        }
    }
}