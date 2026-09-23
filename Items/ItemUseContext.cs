using System;

namespace GameEngine
{
    public sealed class ItemUseContext
    {
        public static ItemUseContext None { get; } =
            new ItemUseContext();

        public Guid? TargetUnitId { get; set; }
    }
}