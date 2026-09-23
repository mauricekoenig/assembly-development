namespace GameEngine
{
    public sealed class BattleWonEvent : GameEvent
    {
        public BattleInfo Battle { get; }

        internal BattleWonEvent(BattleInfo battle)
        {
            Guard.NotNull(battle, nameof(battle));

            Battle = battle;
        }
    }
}