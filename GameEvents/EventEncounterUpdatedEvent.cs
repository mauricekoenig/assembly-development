namespace GameEngine
{
    public sealed class EventEncounterUpdatedEvent : GameEvent
    {
        public EventEncounterInfo Encounter { get; }

        internal EventEncounterUpdatedEvent(
            EventEncounterInfo encounter)
        {
            Guard.NotNull(encounter, nameof(encounter));
            Encounter = encounter;
        }
    }
}
