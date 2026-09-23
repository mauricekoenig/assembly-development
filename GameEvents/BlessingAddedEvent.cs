namespace GameEngine
{
    public sealed class BlessingAddedEvent : GameEvent
    {
        public BlessingInfo Blessing { get; }

        internal BlessingAddedEvent(BlessingInfo blessing)
        {
            Guard.NotNull(blessing, nameof(blessing));

            Blessing = blessing;
        }
    }
}