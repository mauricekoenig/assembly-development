namespace GameEngine
{
    public sealed class BattleLostEvent : GameEvent
    {
        public BattleInfo Battle { get; }

        internal BattleLostEvent(BattleInfo battle)
        {
            Guard.NotNull(battle, nameof(battle));

            Battle = battle;
        }
    }
}