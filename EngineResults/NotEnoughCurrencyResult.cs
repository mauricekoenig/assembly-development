namespace GameEngine
{
    public sealed class NotEnoughCurrencyResult : EngineFailedResult
    {
        public NotEnoughCurrencyResult()
            : base("Not enough currency to purchase this offer.")
        {
        }
    }
}