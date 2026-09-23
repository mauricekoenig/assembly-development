namespace GameEngine
{
    public sealed class ShopEncounterStartedResult : EngineSuccessResult
    {
        public ShopEncounterStartedResult(string shopName)
            : base($"Shop '{shopName}' started successfully.")
        {
        }
    }
}