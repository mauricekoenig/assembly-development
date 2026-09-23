using System;
using System.Collections.Generic;

namespace GameEngine
{
    public abstract class Relic :
        IGameEntity,
        IEffectSource
    {
        private readonly List<TriggeredEffect> _effects =
            new List<TriggeredEffect>();

        public Guid Id { get; } =
            Guid.NewGuid();

        public string DefinitionId { get; }

        public string Name { get; }

        public string Description { get; }

        public IReadOnlyList<TriggeredEffect> Effects =>
            _effects;

        protected Relic(
            string definitionId,
            string name,
            string description)
        {
            Guard.NotNullOrWhiteSpace(
                definitionId,
                nameof(definitionId));

            Guard.NotNullOrWhiteSpace(
                name,
                nameof(name));

            DefinitionId = definitionId;
            Name = name;
            Description = description ?? string.Empty;
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
