namespace GameEngine
{
    public sealed class BlessingEffectDefinition
    {
        public BlessingEffectType Type { get; set; }

        public int Amount { get; set; }

        public SectorType? Sector { get; set; }
    }
}
