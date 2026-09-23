namespace GameEngine
{
    public sealed class StageCompletedEvent : GameEvent
    {
        public int StageIndex { get; }

        public int StageNumber =>
            StageIndex + 1;

        public RunInfo Run { get; }

        internal StageCompletedEvent(
            int stageIndex,
            RunInfo run)
        {
            if (stageIndex < 0)
            {
                throw new System.ArgumentOutOfRangeException(
                    nameof(stageIndex));
            }

            Guard.NotNull(
                run,
                nameof(run));

            StageIndex =
                stageIndex;

            Run =
                run;
        }
    }
}
