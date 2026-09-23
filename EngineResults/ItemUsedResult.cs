namespace GameEngine
{
    public sealed class ItemUsedResult :
        EngineSuccessResult
    {
        internal ItemUsedResult(
            string itemName)
            : base(
                $"Item '{itemName}' was used.")
        {
        }
    }
}