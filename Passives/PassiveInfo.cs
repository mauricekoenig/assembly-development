namespace GameEngine
{
    public sealed class PassiveInfo
    {
        public string Name { get; }
        public string Description { get; }

        internal PassiveInfo(Passive passive)
        {
            Guard.NotNull(passive, nameof(passive));

            Name = passive.Name;
            Description = passive.Description;
        }

        public override string ToString()
        {
            return $"{Name}: {Description}";
        }
    }
}