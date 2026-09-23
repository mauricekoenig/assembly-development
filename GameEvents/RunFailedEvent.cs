namespace GameEngine
{
    public sealed class RunFailedEvent : GameEvent
    {
        public RunInfo Run { get; }

        internal RunFailedEvent(RunInfo run)
        {
            Guard.NotNull(run, nameof(run));

            Run = run;
        }
    }
}