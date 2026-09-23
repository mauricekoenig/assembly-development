namespace GameEngine
{
    public sealed class ShopOfferNotFoundResult : EngineFailedResult
    {
        public ShopOfferNotFoundResult()
            : base("The selected shop offer could not be found.")
        {
        }
    }
}