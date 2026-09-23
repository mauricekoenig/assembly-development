using System.Collections.Generic;

namespace GameEngine
{
    public sealed class Effect
    {
        private readonly List<EffectComponent> _components = new List<EffectComponent>();

        public string Name { get; }
        public string Description { get; }

        public IReadOnlyList<EffectComponent> Components =>
            _components;

        internal Effect(
            string name,
            string description)
        {
            Guard.NotNullOrWhiteSpace(name, nameof(name));
            Guard.NotNullOrWhiteSpace(description, nameof(description));

            Name = name;
            Description = description;
        }

        internal void AddComponent(EffectComponent component)
        {
            Guard.NotNull(component, nameof(component));

            _components.Add(component);
        }
    }
}