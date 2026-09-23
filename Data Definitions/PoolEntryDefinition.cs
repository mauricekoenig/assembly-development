namespace GameEngine
{
    public sealed class PoolEntryDefinition
    {
        public PoolEntryType Type { get; set; }
        public string ContentId { get; set; }
        public int Weight { get; set; } = 1;
    }
}
