namespace GameEngine
{
    public sealed class RelicAddedResult : EngineSuccessResult
    {
        public RelicAddedResult(
            Relic relic)
            : base(
                $"'{relic.Name}' added successfully.")
        {
        }
    }
}
