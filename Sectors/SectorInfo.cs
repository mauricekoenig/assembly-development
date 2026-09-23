using GameEngine;

public sealed class SectorInfo
{
    public SectorType Type { get; }

    internal SectorInfo(Sector sector)
    {
        Guard.NotNull(sector, nameof(sector));

        Type = sector.SectorType;
    }

    public override string ToString()
    {
        return Type.ToString();
    }
}