namespace GameEngine
{
    public sealed class UnitAddedResult : EngineSuccessResult
    {
        public UnitAddedResult(Unit unit)
            : base($"'{unit.Name}' added to the team.")
        {
        }
    }
}