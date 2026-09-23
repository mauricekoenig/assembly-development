using GameEngine;

public sealed class SectorLayoutInfo
{
    public SectorInfo Top { get; }
    public SectorInfo Right { get; }
    public SectorInfo Bottom { get; }
    public SectorInfo Left { get; }

    internal SectorLayoutInfo(SectorLayout layout)
    {
        Guard.NotNull(layout, nameof(layout));

        Top = GetInfo(layout, RingPosition.Top);
        Right = GetInfo(layout, RingPosition.Right);
        Bottom = GetInfo(layout, RingPosition.Bottom);
        Left = GetInfo(layout, RingPosition.Left);
    }

    private static SectorInfo GetInfo(
        SectorLayout layout,
        RingPosition position)
    {
        if (!layout.TryGetSector(position, out Sector sector))
            return null;

        return new SectorInfo(sector);
    }

    public override string ToString()
    {
        return
            $"Top:    {Top?.Type.ToString() ?? "Empty"}\n" +
            $"Right:  {Right?.Type.ToString() ?? "Empty"}\n" +
            $"Bottom: {Bottom?.Type.ToString() ?? "Empty"}\n" +
            $"Left:   {Left?.Type.ToString() ?? "Empty"}";
    }
}