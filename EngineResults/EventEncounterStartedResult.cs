namespace GameEngine
{
    public sealed class EventEncounterStartedResult : EngineSuccessResult
    {
        public EventEncounterStartedResult(string eventName)
            : base($"Event '{eventName}' started successfully.")
        {
        }
    }
}
