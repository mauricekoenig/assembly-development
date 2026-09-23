namespace GameEngine
{
    public sealed class EffectInfo
    {
        public string Name { get; }
        public string Description { get; }

        internal EffectInfo(Effect effect)
        {
            Guard.NotNull(effect, nameof(effect));

            Name = effect.Name;
            Description = effect.Description;
        }

        public override string ToString()
        {
            return $"{Name}: {Description}";
        }
    }
}