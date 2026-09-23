namespace GameEngine
{
    public sealed class BlessingAddedResult : EngineSuccessResult
    {
        public BlessingAddedResult (IBlessing blessing)
            : base($"'{blessing.Name}' added successfully.")
        {
        }
    }
}