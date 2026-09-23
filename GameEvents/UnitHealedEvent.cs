using GameEngine;

public sealed class UnitHealedEvent : GameEvent
{
    public UnitInfo Unit { get; }
    public int Amount { get; }

    internal UnitHealedEvent(
        UnitInfo unit,
        int amount)
    {
        Guard.NotNull(unit, nameof(unit));

        Unit = unit;
        Amount = amount;
    }
}