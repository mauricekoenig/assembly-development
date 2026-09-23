using System;

namespace GameEngine
{
    public sealed class BlessingInfo
    {
        public Guid Id { get; }

        public string DefinitionId { get; }

        public string Name { get; }

        public string Description { get; }


        internal BlessingInfo(
            IBlessing blessing)
        {
            Guard.NotNull(
                blessing,
                nameof(blessing));

            Id = blessing.Id;
            DefinitionId = blessing.DefinitionId;
            Name = blessing.Name;
            Description = blessing.Description;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
