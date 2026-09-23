namespace GameEngine
{
    public sealed class CurrencyLostEvent : GameEvent
    {
        public int Amount { get; }
        public int NewTotal { get; }

        internal CurrencyLostEvent(
            int amount,
            int newTotal)
        {
            Amount = amount;
            NewTotal = newTotal;
        }
    }
}
