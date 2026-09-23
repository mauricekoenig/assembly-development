namespace GameEngine
{
    public sealed class RepositionFailedResult :
        EngineFailedResult
    {
        internal RepositionFailedResult()
            : base(
                "The ring could not be repositioned.")
        {
        }
    }
}