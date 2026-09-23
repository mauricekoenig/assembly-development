using System.Collections.Generic;

namespace GameEngine
{
    public sealed class PoolDefinition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public PoolKind Kind { get; set; } =
            PoolKind.Unspecified;

        public List<PoolEntryDefinition> Entries { get; set; }
    }
}
