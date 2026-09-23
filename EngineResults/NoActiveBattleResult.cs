namespace GameEngine
{
    public sealed class NoActiveBattleResult : EngineFailedResult
    {
        public NoActiveBattleResult()
            : base("There is no active battle.")
        {
        }
    }
}