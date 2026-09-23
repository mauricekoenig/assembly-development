namespace GameEngine
{
    public sealed class ItemUseFailedResult :
        EngineFailedResult
    {
        internal ItemUseFailedResult(
            string message)
            : base(message)
        {
        }
    }
}