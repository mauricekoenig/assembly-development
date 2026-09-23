namespace GameEngine
{
    public sealed class BattleStartedEvent : GameEvent
    {
        public BattleInfo Battle { get; }

        internal BattleStartedEvent(BattleInfo battle)
        {
            Guard.NotNull(battle, nameof(battle));

            Battle = battle;
        }
    }
}