namespace GameEngine
{
    public sealed class BattleStartedResult : EngineSuccessResult
    {
        public BattleStartedResult(Enemy enemy)
            : base($"Battle against '{enemy.Name}' started successfully.")
        {
        }
    }
}