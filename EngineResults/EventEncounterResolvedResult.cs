

namespace GameEngine
{
    public sealed class EventEncounterResolvedResult
    : EngineSuccessResult
    {
        public EventEncounterResolvedResult(string eventName)
            : base($"Event '{eventName}' resolved successfully.")
        {
        }
    }
}