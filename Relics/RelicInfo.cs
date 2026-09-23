using System;

namespace GameEngine
{
    public sealed class RelicInfo
    {
        public Guid Id { get; }

        public string DefinitionId { get; }

        public string Name { get; }

        public string Description { get; }


        internal RelicInfo(
            Relic relic)
        {
            Guard.NotNull(
                relic,
                nameof(relic));

            Id = relic.Id;
            DefinitionId = relic.DefinitionId;
            Name = relic.Name;
            Description = relic.Description;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
