using System.Collections.Generic;

namespace GameEngine
{
    public class Passive
    {
        private readonly List<PassiveModifier> _modifiers =
            new List<PassiveModifier>();

        private readonly List<TriggeredEffect> _effects =
            new List<TriggeredEffect>();

        public string Name { get; }

        public string Description { get; }

        public IReadOnlyList<PassiveModifier> Modifiers =>
            _modifiers;

        public IReadOnlyList<TriggeredEffect> Effects =>
            _effects;

        public Passive(
            string name,
            string description)
        {
            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            Guard.NotNullOrWhiteSpace(
                description,
                nameof(description));

            Name =
                name;

            Description =
                description;
        }

        internal void AddModifier(
            PassiveModifier modifier)
        {
            Guard.NotNull(
                modifier,
                nameof(modifier));

            _modifiers.Add(
                modifier);
        }

        internal void AddEffect(
            TriggeredEffect effect)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            _effects.Add(
                effect);
        }
    }
}
