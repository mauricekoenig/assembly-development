namespace GameEngine
{
    public sealed class UnitChoiceRequiredResult : EngineFailedResult
    {
        public UnitChoiceRequiredResult()
            : base("A Unit choice must be selected before continuing.")
        {
        }
    }
}