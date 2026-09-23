namespace GameEngine
{
    public sealed class NoActiveEventEncounterResult
        : EngineFailedResult
    {
        public NoActiveEventEncounterResult()
            : base("There is no active event encounter.")
        {
        }
    }
}