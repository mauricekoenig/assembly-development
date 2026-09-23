namespace GameEngine
{
    public sealed class ShopEncounterAlreadyActiveResult
        : EngineFailedResult
    {
        public ShopEncounterAlreadyActiveResult()
            : base("A shop encounter is already active.")
        {
        }
    }
}