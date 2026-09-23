namespace GameEngine
{
    public sealed class ShopOfferPurchasedResult : EngineSuccessResult
    {
        public ShopOfferPurchasedResult(string offerName)
            : base($"Shop offer '{offerName}' purchased successfully.")
        {
        }
    }
}