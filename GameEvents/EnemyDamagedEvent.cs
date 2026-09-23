namespace GameEngine
{
    public sealed class EnemyDamagedEvent : GameEvent
    {
        public EnemyInfo Enemy { get; }
        public int Amount { get; }

        internal EnemyDamagedEvent(
            EnemyInfo enemy,
            int amount)
        {
            Guard.NotNull(enemy, nameof(enemy));

            Enemy = enemy;
            Amount = amount;
        }
    }
}