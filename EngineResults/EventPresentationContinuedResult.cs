namespace GameEngine
{
    public sealed class EventPresentationContinuedResult
        : EngineSuccessResult
    {
        public string NodeId { get; }


        public EventPresentationContinuedResult(
            string nodeId)
            : base(
                $"Event Presentation '{nodeId}' continued.")
        {
            NodeId =
                nodeId ??
                string.Empty;
        }
    }
}