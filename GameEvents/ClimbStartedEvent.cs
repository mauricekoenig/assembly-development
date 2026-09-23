namespace GameEngine
{
    public sealed class ClimbStartedEvent : GameEvent
    {
        public RunInfo Run { get; }

        internal ClimbStartedEvent(RunInfo run)
        {
            Guard.NotNull(run, nameof(run));

            Run = run;
        }
    }
}