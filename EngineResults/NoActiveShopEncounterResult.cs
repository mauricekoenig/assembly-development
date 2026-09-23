namespace GameEngine
{
    public sealed class NoActiveShopEncounterResult : EngineFailedResult
    {
        public NoActiveShopEncounterResult()
            : base("There is no active shop encounter.")
        {
        }
    }
}