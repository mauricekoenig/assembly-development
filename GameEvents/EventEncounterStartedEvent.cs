namespace GameEngine
{
    public sealed class EventEncounterStartedEvent : GameEvent
    {
        public EventEncounterInfo Encounter { get; }

        internal EventEncounterStartedEvent(
            EventEncounterInfo encounter)
        {
            Guard.NotNull(encounter, nameof(encounter));
            Encounter = encounter;
        }
    }
}
