namespace GameEngine
{
    public sealed class FloorGenerationFailedResult :
        EngineFailedResult
    {
        public FloorGenerationFailedResult()
            : base("No Floors could be generated.")
        {
        }
    }
}