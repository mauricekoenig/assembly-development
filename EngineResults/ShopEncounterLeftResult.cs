namespace GameEngine
{
    public sealed class ShopEncounterLeftResult : EngineSuccessResult
    {
        public ShopEncounterLeftResult(string shopName)
            : base($"Shop '{shopName}' left successfully.")
        {
        }
    }
}