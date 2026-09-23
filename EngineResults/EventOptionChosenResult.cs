namespace GameEngine
{
    public sealed class EventOptionChosenResult
        : EngineSuccessResult
    {
        public EventOptionChosenResult(string optionName)
            : base($"Event option '{optionName}' chosen successfully.")
        {
        }
    }
}