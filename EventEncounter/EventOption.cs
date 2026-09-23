using System;

namespace GameEngine
{
    internal sealed class EventOption : IGameEntity
    {
        public Guid Id { get; } = Guid.NewGuid();

        internal string DefinitionId { get; }
        internal string Name { get; }
        internal string Description { get; }

        internal EventOptionDefinition Definition { get; }

        internal EventOption(
            EventOptionDefinition definition)
        {
            Guard.NotNull(definition, nameof(definition));
            Guard.NotNullOrWhiteSpace(definition.Id, nameof(definition.Id));
            Guard.NotNullOrWhiteSpace(definition.Name, nameof(definition.Name));

            Definition = definition;
            DefinitionId = definition.Id;
            Name = definition.Name;
            Description = definition.Description ?? string.Empty;
        }
    }
}
