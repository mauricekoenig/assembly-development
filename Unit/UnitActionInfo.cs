using System;

namespace GameEngine
{
    public sealed class UnitActionInfo
    {
        public Guid Id { get; }

        public string DefinitionId { get; }

        public string Name { get; }

        public string Description { get; }


        internal UnitActionInfo(
            UnitAction action)
        {
            Guard.NotNull(
                action,
                nameof(action));

            Id =
                action.Id;

            DefinitionId =
                action.DefinitionId;

            Name =
                action.Name;

            Description =
                action.Description;
        }


        public override string ToString()
        {
            return $"{Name}: {Description}";
        }
    }
}
