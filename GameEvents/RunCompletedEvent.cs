namespace GameEngine
{
    public sealed class RunCompletedEvent : GameEvent
    {
        public RunInfo Run { get; }

        internal RunCompletedEvent(RunInfo run)
        {
            Guard.NotNull(run, nameof(run));
            Run = run;
        }
    }
}