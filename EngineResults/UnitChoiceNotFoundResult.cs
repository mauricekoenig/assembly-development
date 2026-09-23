namespace GameEngine
{
    public sealed class UnitChoiceNotFoundResult : EngineFailedResult
    {
        public UnitChoiceNotFoundResult()
            : base("The selected Unit choice was not found.")
        {
        }
    }
}