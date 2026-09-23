using GameEngine;

public sealed class EnemyHealedEvent : GameEvent
{
    public EnemyInfo Enemy { get; }
    public int Amount { get; }

    internal EnemyHealedEvent(
        EnemyInfo enemy,
        int amount)
    {
        Guard.NotNull(enemy, nameof(enemy));

        Enemy = enemy;
        Amount = amount;
    }
}