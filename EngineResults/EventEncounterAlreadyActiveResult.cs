namespace GameEngine
{
    public sealed class EventEncounterAlreadyActiveResult
        : EngineFailedResult
    {
        public EventEncounterAlreadyActiveResult()
            : base("An event encounter is already active.")
        {
        }
    }
}