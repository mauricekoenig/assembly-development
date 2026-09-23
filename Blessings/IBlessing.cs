using System;

namespace GameEngine
{
    public interface IBlessing
    {
        Guid Id { get; }

        string DefinitionId { get; }

        string Name { get; }

        string Description { get; }

        void Apply(Run run);
    }
}
