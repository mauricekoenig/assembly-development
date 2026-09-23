using System;
using System.Collections.Generic;

namespace GameEngine
{
    public sealed class UnitAction :
        IGameEntity
    {
        private readonly List<ActionEffect> _effects =
            new List<ActionEffect>();

        public Guid Id { get; } =
            Guid.NewGuid();

        public string DefinitionId { get; }

        public string Name { get; }

        public string Description { get; }

        public IReadOnlyList<ActionEffect> Effects =>
            _effects;

        internal UnitAction(
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

            Guard.NotNullOrWhiteSpace(
                description,
                nameof(description));

            DefinitionId =
                definitionId;

            Name =
                name;

            Description =
                description;
        }

        internal void AddEffect(
            ActionEffect effect)
        {
            Guard.NotNull(
                effect,
                nameof(effect));

            _effects.Add(
                effect);
        }
    }
}
