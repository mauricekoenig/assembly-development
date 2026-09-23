namespace GameEngine
{
    public sealed class EventActivityCompletedResult : EngineSuccessResult
    {
        public string ActivityId { get; }
        public string ResultId { get; }

        public EventActivityCompletedResult(
            string activityId,
            string resultId)
            : base($"Event activity '{activityId}' completed with result '{resultId}'.")
        {
            ActivityId = activityId ?? string.Empty;
            ResultId = resultId ?? string.Empty;
        }
    }
}
