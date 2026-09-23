namespace GameEngine
{
    public sealed class BattleAlreadyActiveResult : EngineFailedResult
    {
        public BattleAlreadyActiveResult()
            : base("A battle is already active.")
        {
        }
    }
}