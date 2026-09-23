namespace GameEngine
{
    public sealed class AbilityUseFailedResult :
        EngineFailedResult
    {
        internal AbilityUseFailedResult()
            : base(
                "The active ability could not be used.")
        {
        }
    }
}