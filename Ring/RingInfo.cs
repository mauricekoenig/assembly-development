namespace GameEngine
{
    public sealed class RingInfo
    {
        public RingObjectInfo Top { get; }
        public RingObjectInfo Right { get; }
        public RingObjectInfo Bottom { get; }
        public RingObjectInfo Left { get; }

        internal RingInfo(Ring ring)
        {
            Guard.NotNull(ring, nameof(ring));

            Top = GetInfo(
                ring,
                RingPosition.Top);

            Right = GetInfo(
                ring,
                RingPosition.Right);

            Bottom = GetInfo(
                ring,
                RingPosition.Bottom);

            Left = GetInfo(
                ring,
                RingPosition.Left);
        }

        private static RingObjectInfo GetInfo(
            Ring ring,
            RingPosition position)
        {
            if (!ring.TryGetObject(
                position,
                out RingObject ringObject))
                return null;

            return ringObject switch
            {
                Unit unit => new UnitInfo(unit),
                _ => new RingObjectInfo(ringObject)
            };
        }

        private static string Format(
            RingObjectInfo info)
        {
            return info?.ToString() ?? "Empty";
        }

        public override string ToString()
        {
            return
                $"Top:    {Format(Top)}\n" +
                $"Right:  {Format(Right)}\n" +
                $"Bottom: {Format(Bottom)}\n" +
                $"Left:   {Format(Left)}";
        }
    }
}