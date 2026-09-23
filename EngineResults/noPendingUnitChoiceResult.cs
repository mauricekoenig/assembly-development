namespace GameEngine
{
    public sealed class NoPendingUnitChoiceResult : EngineFailedResult
    {
        public NoPendingUnitChoiceResult()
            : base("There is currently no Unit choice to select.")
        {
        }
    }
}