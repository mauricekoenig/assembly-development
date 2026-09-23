namespace GameEngine
{
    public sealed class RunStartedEvent : GameEvent
    {
        public RunInfo Run { get; }

        internal RunStartedEvent(RunInfo run)
        {
            Guard.NotNull(run, nameof(run));

            Run = run;
        }
    }
}