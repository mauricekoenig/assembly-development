using System;

namespace GameEngine
{
    public abstract class BaseBlessing : IBlessing
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string DefinitionId { get; }

        public string Name { get; }

        public string Description { get; }


        protected BaseBlessing(
            string name)
            : this(
                null,
                name,
                null)
        {
        }


        protected BaseBlessing(
            string definitionId,
            string name,
            string description)
        {
            DefinitionId = definitionId;
            Name = name;
            Description = description;
        }


        public abstract void Apply(Run run);
    }
}
