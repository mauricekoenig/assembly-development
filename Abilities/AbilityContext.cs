using System;

namespace GameEngine
{
    public sealed class AbilityContext
    {
        public static AbilityContext None { get; } =
            new AbilityContext();

        public Guid? TargetUnitId { get; set; }

        public Guid? SecondUnitId { get; set; }

        public RingPosition? TargetPosition { get; set; }
    }
}