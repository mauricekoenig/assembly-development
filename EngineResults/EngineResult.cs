namespace GameEngine
{
    public abstract class EngineResult
    {
        public bool Success { get; }
        public string Message { get; }

        protected EngineResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }

    public abstract class EngineSuccessResult : EngineResult
    {
        protected EngineSuccessResult(string message)
            : base(true, message)
        {
        }
    }

    public abstract class EngineFailedResult : EngineResult
    {
        protected EngineFailedResult(string message)
            : base(false, message)
        {
        }
    }
}