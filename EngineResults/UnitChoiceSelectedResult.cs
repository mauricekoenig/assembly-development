namespace GameEngine
{
    public sealed class UnitChoiceSelectedResult : EngineSuccessResult
    {
        public UnitChoiceSelectedResult(Unit unit)
            : base($"'{unit.Name}' selected successfully.")
        {
        }
    }
}