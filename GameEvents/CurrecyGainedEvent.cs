namespace GameEngine
{
    public sealed class CurrencyGainedEvent : GameEvent
    {
        public int Amount { get; }
        public int NewTotal { get; }

        internal CurrencyGainedEvent(
            int amount,
            int newTotal)
        {
            Amount = amount;
            NewTotal = newTotal;
        }
    }
}